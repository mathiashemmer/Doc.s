using Docs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRESt.Models
{
    public interface IDocumentRepository
    {
        Task<IEModel<DocumentModel>> Add(DocumentModel document);
        Task<IEModel<DocumentModel>> Update(DocumentModel document);
        Task<IEModel<DocumentModel>> Delete(int ID);
        Task<IEModel<DocumentModel>> Find(int ID);
        Task<IEModel<IEnumerable<DocumentModel>>> FindAll();
        Task<IEModel<DocumentModel>> Download(int ID);
    }
}
