using Docs.Models;
using DocsRESt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRegister.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DocumentDBContext context;

        public CategoryRepository(DocumentDBContext context)
        {
            this.context = context;
        }

        public async Task<IEModel<CategoryModel>> Add(CategoryModel category)
        {
            try
            {
                EntityEntry<CategoryModel> addedCategory = await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return new IEModel<CategoryModel>("", CustomErrorCode.Sucess, addedCategory.Entity);
            }
            catch (DbUpdateException ex)
            {
                return new IEModel<CategoryModel>($"Erro ao salvar adição: {ex.Message}", CustomErrorCode.NoOperation, (CategoryModel)(ex.Entries.First().Entity));
            }
        }

        public async Task<IEModel<CategoryModel>> Delete(int ID)
        {
            try
            {
                CategoryModel categoryToDelete = await context.Categories.FindAsync(ID);
                if (categoryToDelete == null) return new IEModel<CategoryModel>("Categoria para deletar não encontrado", CustomErrorCode.KeyNotFound, null);

                context.Categories.Remove(categoryToDelete);
                await context.SaveChangesAsync();
                return new IEModel<CategoryModel>("", CustomErrorCode.Sucess, categoryToDelete);
            }
            catch (DbUpdateException ex)
            {
                return new IEModel<CategoryModel>($"Erro ao deletar categoria: {ex.Message}", CustomErrorCode.NoOperation, (CategoryModel)(ex.Entries.First().Entity));
            }
        }

        public async Task<IEModel<CategoryModel>> Find(int ID)
        {
            try
            {
                CategoryModel category = await context.Categories.FindAsync(ID);
                if (category == null) return new IEModel<CategoryModel>("Categoria não encontrado", CustomErrorCode.KeyNotFound, null);
                return new IEModel<CategoryModel>("", CustomErrorCode.Sucess, category);
            }
            catch (Exception ex)
            {
                return new IEModel<CategoryModel>(ex.Message, CustomErrorCode.NoOperation, null);
            }
        }

        public async Task<IEModel<IEnumerable<CategoryModel>>> FindAll()
        {
            try
            {
                IEnumerable<CategoryModel> categories = await context.Categories.ToListAsync();
                if (categories == null) return new IEModel<IEnumerable<CategoryModel>>("Não foi possivel localizar as categorias", CustomErrorCode.KeyNotFound, null);
                return new IEModel<IEnumerable<CategoryModel>>("", CustomErrorCode.Sucess, categories);
            }
            catch (Exception ex)
            {
                return new IEModel<IEnumerable<CategoryModel>>(ex.Message, CustomErrorCode.NoOperation, null);
            }
        }

        public async Task<IEModel<CategoryModel>> Update(CategoryModel category)
        {
            try
            {
                CategoryModel categoryToUpdate = await context.Categories.FindAsync(category.ID);
                if (categoryToUpdate == null) return new IEModel<CategoryModel>("Categoria para atualizar não encontrado", CustomErrorCode.KeyNotFound, null);

                categoryToUpdate.NAME = category.NAME;

                await context.SaveChangesAsync();
                return new IEModel<CategoryModel>("", CustomErrorCode.Sucess, categoryToUpdate);
            }
            catch (DbUpdateException ex)
            {
                return new IEModel<CategoryModel>($"Erro ao salvar atualização: {ex.Message}", CustomErrorCode.NoOperation, (CategoryModel)(ex.Entries.First().Entity));
            }
        }
    }
}
