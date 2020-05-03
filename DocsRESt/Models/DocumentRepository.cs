using Docs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace DocsRESt.Models
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly DocumentDBContext context;
        public DocumentRepository(DocumentDBContext context)
        {
            this.context = context;
        }
        public async Task<IEModel<DocumentModel>> Add(DocumentModel document)
        {
            FileModel file = document.FILE;

            EntityEntry<FileModel> addedFile = null;
            EntityEntry<DocumentModel> addedDocument = null;

            try
            {
                addedFile = await context.Files.AddAsync(file);
                await context.SaveChangesAsync();

                document.FILE_ID = addedFile.Entity.ID;
                addedDocument = await context.Documents.AddAsync(document);
                await context.SaveChangesAsync();

                addedDocument.Entity.FILE = null;
                return new IEModel<DocumentModel>("", CustomErrorCode.Sucess, addedDocument.Entity); ;
            } catch (DbUpdateException ex)
            {
                // On error, undo added registers
                if (addedFile != null) context.Files.Remove(addedFile.Entity);
                if (addedDocument != null) context.Documents.Remove(addedDocument.Entity);
                await context.SaveChangesAsync();

                if(ex.HResult == -2146233088) return new IEModel<DocumentModel>($"Código duplicado [{document.CODE}] ", CustomErrorCode.FieldDuplicated, document);

                //TODO (IEModel 001)
                // Change return payload to accept multiple paylaods (Even though in this situation, only one document should be returned)
                return new IEModel<DocumentModel>($"Erro ao salvar adição: {ex.Message}", CustomErrorCode.NoOperation, (DocumentModel)(ex.Entries.First().Entity));
            }
        }

        public async Task<IEModel<DocumentModel>> Find(int ID)
        {
            try
            {
                DocumentModel document = await context.Documents.FindAsync(ID);
                if (document == null) return new IEModel<DocumentModel>("Documento não encontrado", CustomErrorCode.KeyNotFound, null);
                return new IEModel<DocumentModel>("", CustomErrorCode.Sucess, document);
            }
            catch (Exception ex)
            {
                return new IEModel<DocumentModel>(ex.Message, CustomErrorCode.NoOperation, null);
            }
            
        }

        public async Task<IEModel<IEnumerable<DocumentModel>>> FindAll()
        {
            try
            {
                IEnumerable<DocumentModel> documents = await context.Documents.ToListAsync();
                if(documents == null) return new IEModel<IEnumerable<DocumentModel>>("Não foi possivel localizar os documentos", CustomErrorCode.KeyNotFound, null);
                return new IEModel<IEnumerable<DocumentModel>>("", CustomErrorCode.Sucess, documents);
            } catch (Exception ex)
            {
                return new IEModel<IEnumerable<DocumentModel>>(ex.Message, CustomErrorCode.NoOperation, null);
            }
        }

        public async Task<IEModel<DocumentModel>> Update(DocumentModel document)
        {
            try
            {
                DocumentModel documentToUpdate = await context.Documents.FindAsync(document.ID);
                if (documentToUpdate == null) return new IEModel<DocumentModel>("Documento para atualizar não encontrado", CustomErrorCode.KeyNotFound, null);

                documentToUpdate.TITLE = document.TITLE;
                documentToUpdate.CATEGORY_ID = document.CATEGORY_ID;
                documentToUpdate.PROCESS = document.PROCESS;
                documentToUpdate.FILE_NAME = document.FILE_NAME;

                await context.SaveChangesAsync();
                return new IEModel<DocumentModel>("", CustomErrorCode.Sucess, documentToUpdate);
            }
            catch (DbUpdateException ex)
            {
                return new IEModel<DocumentModel>($"Erro ao salvar atualização: {ex.Message}", CustomErrorCode.NoOperation, (DocumentModel)(ex.Entries.First().Entity));
            }
        }

        public async Task<IEModel<DocumentModel>> Delete(int ID)
        {
            try
            {
                DocumentModel documentToDelete = await context.Documents.FindAsync(ID);
                if (documentToDelete == null) return new IEModel<DocumentModel>("Documento para deletar não encontrado", CustomErrorCode.KeyNotFound, null);
                context.Documents.Remove(documentToDelete);

                string returnMessage;
                CustomErrorCode errNo;
                FileModel fileToDelete = await context.Files.FindAsync(documentToDelete.FILE_ID);
                if (fileToDelete == null)
                {
                    returnMessage = "Documento excluido, porém não foi localido nenhum arquivo referente ao documento!";
                    errNo = CustomErrorCode.Sucess;
                }
                else
                {
                    context.Files.Remove(fileToDelete);
                    returnMessage = "";
                    errNo = CustomErrorCode.Sucess;
                }
                await context.SaveChangesAsync();
                return new IEModel<DocumentModel>(returnMessage, errNo, documentToDelete);
            }
            catch(DbUpdateException ex)
            {
                return new IEModel<DocumentModel>($"Erro ao deletar documento: {ex.Message}", CustomErrorCode.NoOperation, (DocumentModel)(ex.Entries.First().Entity));
            }    
        }

        public async Task<IEModel<DocumentModel>> Download(int ID)
        {
            try
            {
                DocumentModel document = await context.Documents.FindAsync(ID);
                if (document == null) return new IEModel<DocumentModel>("Documento não encontrado", CustomErrorCode.KeyNotFound, null);
                if (document.FILE_ID == 0) return new IEModel<DocumentModel>("Documento não associado a nenhum arquivo", CustomErrorCode.KeyNotFound, null);

                FileModel file = await context.Files.FindAsync(document.FILE_ID);
                if (file == null || file.FILE == null) new IEModel<DocumentModel>("Arquivo vazio", CustomErrorCode.NoOperation, document);

                document.FILE = file;
                return new IEModel<DocumentModel>("", CustomErrorCode.Sucess, document);
            }
            catch (Exception ex)
            {
                return new IEModel<DocumentModel>(ex.Message, CustomErrorCode.NoOperation, null);
            }
        }
    }
}
