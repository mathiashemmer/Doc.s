using Docs.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DocsBlazor.Services
{
    public class CategoryService
    {
        private readonly HttpClient httpClient;

        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoryList()
        {
            try
            {
                IEModel<IEnumerable<CategoryModel>> response = await httpClient.GetJsonAsync<IEModel<IEnumerable<CategoryModel>>>("api/category");
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryModel> CreateCategory(CategoryModel category)
        {
            try
            {
                IEModel<CategoryModel> response = await httpClient.PostJsonAsync<IEModel<CategoryModel>>("api/category", category);
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryModel> UpdateCategory(CategoryModel category)
        {
            try
            {
                IEModel<CategoryModel> response = await httpClient.PutJsonAsync<IEModel<CategoryModel>>("api/category", category);
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryModel> DeleteCategory(int ID)
        {
            try
            {
                HttpResponseMessage httpResponse = await httpClient.DeleteAsync("api/category/" + ID);
                IEModel<CategoryModel> response = JsonConvert.DeserializeObject<IEModel<CategoryModel>>(await httpResponse.Content.ReadAsStringAsync());
                if (response.ErrorStatusCode != CustomErrorCode.Sucess) throw new Exception(response.CustomMessage);

                return response.Payload;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }
    }
}
