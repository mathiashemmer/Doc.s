using Docs.Models;
using DocsRESt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DocsRESt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository DocumentRepository;
        public DocumentController(IDocumentRepository DocumentRepository)
        {
            this.DocumentRepository = DocumentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEModel<IEnumerable<DocumentModel>>>> GetDocuments()
        {
            try
            {
                return Ok(await this.DocumentRepository.FindAll());
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<DocumentModel>($"Internal Server Error {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpPost()]
        public async Task<ActionResult<IEModel<DocumentModel>>> CreateDocument([FromBody] DocumentModel document)
        {
            try
            {
                if (document == null) return Ok(new IEModel<DocumentModel>("Parametro inválido", CustomErrorCode.NullParameter, null));

                return Ok(await this.DocumentRepository.Add(document));
            }
            catch (Exception e)
            {
                return Ok(new IEModel<DocumentModel>($"Internal Server Error: {e.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpPut]
        public async Task<ActionResult<IEModel<DocumentModel>>> UpdateDocument([FromBody] DocumentModel document)
        {
            try
            {
                if (document == null) return Ok(new IEModel<DocumentModel>("Parametro inválido", CustomErrorCode.NullParameter, null));

                return Ok(await this.DocumentRepository.Update(document));
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<DocumentModel>($"Internal Server Error: {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpDelete("{ID:int}")]
        public async Task<ActionResult<IEModel<DocumentModel>>> DeleteDocument(int ID)
        {
            try
            {
                if (ID == 0) return Ok(new IEModel<DocumentModel>("Parametro inválido", CustomErrorCode.NullParameter, null));

                return Ok(await this.DocumentRepository.Delete(ID));
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<DocumentModel>($"Internal Server Error: {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpGet("download/{ID:int}")]
        public async Task<ActionResult<IEModel<DocumentModel>>> Download(int ID)
        {
            try
            {
                if (ID == 0) return Ok(new IEModel<DocumentModel>("Parametro inválido", CustomErrorCode.NullParameter, null));

                return Ok(await this.DocumentRepository.Download(ID));
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<DocumentModel>($"Internal Server Error: {ex.Message}", CustomErrorCode.NoOperation, null));
            } 
        }
    }
}
