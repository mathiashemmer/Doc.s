using Docs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRegister.Repository
{
    public interface ICategoryRepository
    {
        Task<IEModel<CategoryModel>> Add(CategoryModel category);
        Task<IEModel<CategoryModel>> Update(CategoryModel category);
        Task<IEModel<CategoryModel>> Delete(int ID);
        Task<IEModel<CategoryModel>> Find(int ID);
        Task<IEModel<IEnumerable<CategoryModel>>> FindAll();
        
    }
}
