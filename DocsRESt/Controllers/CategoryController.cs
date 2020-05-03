using Docs.Models;
using DocumentRegister.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRESt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository CategoryRepository;
        public CategoryController(ICategoryRepository CategoryRepository)
        {
            this.CategoryRepository = CategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEModel<IEnumerable<CategoryModel>>>> GetCategories()
        {
            try
            {
                return Ok(await CategoryRepository.FindAll());
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<CategoryModel>($"Internal Server Error {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpGet("{ID:int}")]
        public async Task<ActionResult<IEModel<CategoryModel>>> GetCategoryByID(int ID)
        {
            try
            {
                if(ID == 0) Ok(new IEModel<CategoryModel>("Parametro inválido", CustomErrorCode.InvalidParameter, null));

                return Ok(await CategoryRepository.Find(ID));
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<CategoryModel>($"Internal Server Error {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEModel<CategoryModel>>> CreateCategory(CategoryModel category)
        {
            try
            {
                if (category == null) return Ok(new IEModel<CategoryModel>("Parametro inválido", CustomErrorCode.NullParameter, null));

                return Ok(await CategoryRepository.Add(category));
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<CategoryModel>($"Internal Server Error {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpDelete("{ID:int}")]
        public async Task<ActionResult<IEModel<CategoryModel>>> DeleteCategory(int ID)
        {
            try
            {
                if (ID == 0) Ok(new IEModel<CategoryModel>("Parametro inválido", CustomErrorCode.InvalidParameter, null));

                return Ok(await CategoryRepository.Delete(ID));
            } catch (Exception ex)
            {
                return Ok(new IEModel<CategoryModel>($"Internal Server Error {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }

        [HttpPut]
        public async Task<ActionResult<IEModel<CategoryModel>>> UpdateCategory(CategoryModel category)
        {
            try
            {
                if (category == null) return Ok(new IEModel<CategoryModel>("Parametro inválido", CustomErrorCode.NullParameter, null));

                return (await this.CategoryRepository.Update(category));
            }
            catch (Exception ex)
            {
                return Ok(new IEModel<CategoryModel>($"Internal Server Error {ex.Message}", CustomErrorCode.NoOperation, null));
            }
        }
    }
}
