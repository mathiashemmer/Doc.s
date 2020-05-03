using Docs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRESt.Models
{
    public class DocumentDBContext : DbContext
    {
        public DocumentDBContext(DbContextOptions<DocumentDBContext> options) : base(options) { }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<DocumentModel> Documents { get; set; }
        public DbSet<FileModel> Files { get; set; }
    }
}
