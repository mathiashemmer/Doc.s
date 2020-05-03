using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Docs.Models;
using DocsBlazor.Const;
using DocsBlazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace DocsBlazor.Pages.Documents
{
    public class DocumentBase : FormControllBase<DocumentModel>
    {
        [Inject]
        DocumentService documentService { get; set; }
        [Inject]
        CategoryService categoryService { get; set; }

        // File handling properties
        protected bool IsUploading { get; set; } = false;
        protected int UploadProgress { get; set; } = 0;

        // Docs handling properties
        protected List<DocumentModel> DocumentList { get; set; } = new List<DocumentModel>();
        protected List<CategoryModel> CategoryList { get; set; }
        protected DocumentModel CurrentDocument { get; set; }
        protected Validations DocumentValidator { get; set; }

        // Base fuctions
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override async Task OnInitializedAsync()
        {
            await this.GetList();
        }

        // Form functions
        protected override async Task GetList()
        {
            try
            {
                this.Block();
                DocumentList = (await documentService.GetDocumentList()).ToList();
                CategoryList = (await categoryService.GetCategoryList()).ToList();
                this.Unblock();
            }
            catch (Exception ex)
            {
                this.ShowToast($"Não foi possível buscar a lista de informações: {ex.Message}", 3000);
                this.Unblock();
            }
        }
        protected string MapCategoryIDToName(int ID)
        {
            if(this.CategoryList != null)
            {
                CategoryModel category = CategoryList.Find(currentCategory => currentCategory.ID == ID);
                if (category != null) return category.NAME;
            }
            return "Categoria Nao Cadastrada!";
        }
        protected override bool OnNew()
        {
            this.CurrentOperation = FormOperation.Adicionando;
            this.CurrentDocument = new DocumentModel();
            CurrentDocument.FILE = new FileModel();
            this.CurrentDocument.FILE_ID = 0;
            if(CategoryList != null && CategoryList.Count() > 0)
                this.CurrentDocument.CATEGORY_ID = CategoryList.First().ID;
            return true;
        }
        protected override bool OnView(DocumentModel document)
        {
            this.CurrentOperation = FormOperation.Visualizando;
            this.CurrentDocument = document;
            return true;
        }
        protected override bool OnEdit(DocumentModel document)
        {
            this.CurrentDocument = new DocumentModel
            {
                ID = document.ID,
                TITLE = document.TITLE,
                CATEGORY_ID = document.CATEGORY_ID,
                PROCESS = document.PROCESS,
                CODE = document.CODE,
                FILE_ID = document.FILE_ID,
                FILE_NAME = document.FILE_NAME,
                FILE_TYPE = document.FILE_TYPE
            };

            this.CurrentOperation = FormOperation.Editando;
            return true;
        }
        protected override bool OnCancel()
        {
            CurrentOperation = FormOperation.Nenhum;
            this.ResetEntries();
            return true;
        }
        protected override async Task<bool> OnSave()
        {
            try
            {
                this.Block();
                bool returnType = false;

                if (!this.ValidateEntries())
                {
                    this.ShowToast("Existem campos preenchidos de forma incorreta!");
                    this.Unblock();
                    return false;
                }

                if (this.CurrentOperation == FormOperation.Editando)
                {
                    DocumentModel updatedDocument = await this.documentService.UpdateDocument(this.CurrentDocument);
                    if(updatedDocument != null)
                    {
                        DocumentModel documentToUpdate = this.DocumentList.Find(document => document.ID == updatedDocument.ID);
                        if(documentToUpdate == null)
                            await this.GetList();
                        else
                        {
                            DocumentList.Remove(documentToUpdate);
                            DocumentList.Add(updatedDocument);
                        }
                        this.ShowToast("Documento editado com sucesso!");
                    }
                    
                }
                else
                {
                    var newDocument = await documentService.CreateDocument(CurrentDocument);
                    if (newDocument != null)
                    {
                        this.DocumentList.Add(newDocument);
                        this.ShowToast("Documento inserido com sucesso!");
                        returnType = true;
                    }
                    else
                    {
                        this.CurrentDocument = null;
                        this.ShowToast("Erro ao inserir categoria!");
                    }
                }

                this.CurrentOperation = FormOperation.Nenhum;
                this.ResetEntries();
                this.Unblock();
                return returnType;
            }
            catch (Exception e)
            { 
                this.ShowToast(e.Message);
                //this.CurrentOperation = FormOperation.Nenhum;
                //this.ResetEntries();
                this.Unblock();
                return false;
            }
        }
        protected override async Task<bool> OnDelete(DocumentModel document)
        {
            try
            {
                if (CurrentDocument == null) return false;
                this.Block();
                DocumentModel deletedDocument = await this.documentService.DeleteDocument(CurrentDocument.ID);
                if (deletedDocument != null)
                {
                    this.ShowToast("Documento deletado com sucesso!");
                    if(!this.DocumentList.Remove(CurrentDocument)) await this.GetList();
                    this.CurrentDocument = null;
                    this.CurrentOperation = FormOperation.Nenhum;
                    this.Unblock();
                    return true;
                }
                else
                {
                    throw new Exception("Erro ao deletar documento");
                }
            }
            catch (Exception ex)
            {
                this.ShowToast(ex.Message);
                this.Unblock();
                this.CurrentOperation = FormOperation.Nenhum;
                this.ResetEntries();
                return false;
            }
        }
        protected async Task OnDownload(DocumentModel document)
        {
            try
            {
                if (this.CurrentOperation != FormOperation.Nenhum) return;
                this.Block();
                await this.documentService.Download(document);

                this.Unblock();
                return;
            }
            catch (Exception e)
            {
                this.ShowToast(e.Message);
                this.Unblock();
                return;
            }
        }
        private bool ValidateEntries()
        {
            return !(
                !DocumentValidator.ValidateAll()
                || this.CurrentDocument == null
                || this.CurrentDocument.FILE_NAME == null
                || this.CurrentDocument.FILE_NAME.Equals("")
                || this.CurrentOperation == FormOperation.Adicionando
                && (this.CurrentDocument.FILE == null
                || this.CurrentDocument.FILE.FILE == null)
                ) ;
        }
        private void ResetEntries()
        {
            this.CurrentDocument = null;
            this.IsUploading = false;
            this.UploadProgress = 0;
        }
        // File Handle Input
        protected void OnFileInputStart()
        {
            this.IsUploading = true;
            this.UploadProgress = 0;
        }
        protected async Task OnFileInputChanged(FileChangedEventArgs e)
        {
            this.Block();
            try
            {
                foreach (var file in e.Files)
                {              
                    using (var stream = new MemoryStream())
                    {
                        this.CurrentDocument.FILE_NAME = file.Name;
                        if (!this.MapFileExtension())
                        {
                            this.CurrentDocument.FILE_NAME = null;
                            this.CurrentDocument.FILE = null;
                            this.IsUploading = false;
                            this.UploadProgress = 0;

                            return;
                        }
                        await file.WriteToStreamAsync(stream);
                        this.CurrentDocument.FILE.FILE = stream.ToArray();
                        
                    }
                }
            }
            catch (Exception)
            {
                this.ShowToast("Erro ao tentar selecionar o arquivo!");
            }
            finally
            {
                this.Unblock();
                this.StateHasChanged();
            }
        }
        protected void OnFileInputWritten(FileWrittenEventArgs e)
        {
        }
        protected void OnFileInputProgess(FileProgressedEventArgs e)
        {
            this.UploadProgress = (int)e.Percentage;
        }
        protected void OnFileInputEnd()
        {
            this.IsUploading = false;
            this.UploadProgress = 100;
            this.ShowToast($"Arquivo carregado com sucesso! Pronto para gravar!");
        }
        private bool MapFileExtension()
        {
            if(this.CurrentDocument != null)
            {
                string extension = Path.GetExtension(this.CurrentDocument.FILE_NAME);
                string formatedExtension = extension.Substring(1).ToUpper();
                string[] acceptedTypes = Enum.GetNames(typeof(FileType));
                foreach(string currentType in acceptedTypes)
                {
                    if (formatedExtension.Equals(currentType))
                    {
                        this.CurrentDocument.FILE_TYPE = (FileType)Enum.Parse(typeof(FileType), formatedExtension);
                        return true;
                    }
                }
                this.ShowToast("Arquivo inválido! Somente: PDF, DOC, DOCX, XLS, XLSX!");
                return false;
            }
            return false;
        }
    }
}
