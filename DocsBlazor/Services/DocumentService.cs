using Docs.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using DocsBlazor.Shared;
using Microsoft.JSInterop;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace DocsBlazor.Services
{
    public class DocumentService
    {

        private Microsoft.JSInterop.IJSRuntime JS { get; set; }

        private readonly HttpClient httpClient;
        public DocumentService(HttpClient httpClient, IJSRuntime js)
        {
            this.httpClient = httpClient;
            this.JS = js;
        }

        public async Task<IEnumerable<DocumentModel>> GetDocumentList()
        {
            try
            {
                IEModel<IEnumerable<DocumentModel>> response = await httpClient.GetJsonAsync<IEModel<IEnumerable<DocumentModel>>>("api/document");
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }
        public async Task<DocumentModel> CreateDocument(DocumentModel document)
        {
            try
            {
                IEModel<DocumentModel> response = await this.httpClient.PostJsonAsync<IEModel<DocumentModel>>("api/document", document);
                if(response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            
        }

        public async Task<DocumentModel> UpdateDocument(DocumentModel document)
        {
            try
            {
                IEModel<DocumentModel> response = await httpClient.PutJsonAsync<IEModel<DocumentModel>>("api/document", document);
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            
        }

        public async Task<DocumentModel> DeleteDocument(int ID)
        {
            try
            {
                HttpResponseMessage httpResponse = await httpClient.DeleteAsync("api/document/" + ID);
                IEModel<DocumentModel> response = JsonConvert.DeserializeObject<IEModel<DocumentModel>>(await httpResponse.Content.ReadAsStringAsync());
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch(HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<DocumentModel> Download(DocumentModel document)
        {
            try
            {
                IEModel<DocumentModel> response = await httpClient.GetJsonAsync<IEModel<DocumentModel>>($"api/document/download/{document.ID}");
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);
                DocumentModel content = response.Payload;
                await JS.SaveAs(content.FILE_NAME, content.FILE.FILE);
                return content;
            }
            catch(HttpRequestException ex)
            {
                throw ex;
            }
        }
    }
}
