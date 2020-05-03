using Blazorise;
using Blazorise.Snackbar;
using Docs.Models;
using DocsBlazor.Const;
using DocsBlazor.Services;
using DocsBlazor.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.CodeAnalysis.Differencing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsBlazor.Pages.Categories
{
    public class CategoryBase : FormControllBase<CategoryModel>
    {
        // Dependencies and Injection properties
        [Inject]
        CategoryService categoryService { get; set; }

        // Data handling properties
        protected List<CategoryModel> CategoryList { get; set; }
        protected CategoryModel CurrentCategory { get; set; }
        protected Validations CategoryValidator { get; set; }

        // Base fuctions
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override async Task OnInitializedAsync()
        {
            await this.GetList();
        }

        // Custom form function
        protected override async Task GetList()
        {
            try
            {
                this.Block();
                CategoryList = (await categoryService.GetCategoryList()).ToList();
                this.Unblock();
            }
            catch (Exception ex)
            {
                this.ShowToast($"Não foi possível buscar a lista de informações: {ex.Message}", 3000);
                this.Unblock();
            }
        }
        protected override bool OnNew()
        {
            this.CurrentOperation = FormOperation.Adicionando;
            this.CurrentCategory = new CategoryModel();
            return true;
        }

        protected override bool OnView(CategoryModel category)
        {
            this.CurrentOperation = FormOperation.Visualizando;
            this.CurrentCategory = category;
            return true;
        }

        protected override bool OnEdit(CategoryModel category)
        {
            this.CurrentCategory = new CategoryModel {
                ID = category.ID,
                NAME = category.NAME
            };

            this.CurrentOperation = FormOperation.Editando;
            return true;
        }

        protected override async Task<bool> OnSave()
        {
            try
            {
                this.Block();
                bool returnType = false;
                if (!this.CategoryValidator.ValidateAll())
                {
                    this.ShowToast("Existem campos preenchidos de forma incorreta!");
                    return false;
                }
                     
                if (this.CurrentOperation == FormOperation.Editando)
                {
                    var updatedCategory = await this.categoryService.UpdateCategory(CurrentCategory);
                    if(updatedCategory != null)
                    {
                        var categoryToUpdate = this.CategoryList.ToList().Find(category => category.ID == updatedCategory.ID);
                        if (categoryToUpdate == null)
                        {
                            await this.GetList();
                        }
                        else
                        {
                            categoryToUpdate.NAME = updatedCategory.NAME;
                        }
                        this.ShowToast("Categorias atualizada com sucesso!");
                    }
                }
                else
                {
                    var newCategory = await categoryService.CreateCategory(CurrentCategory);
                    if (newCategory != null)
                    {
                        ((List<CategoryModel>)this.CategoryList).Add(newCategory);
                        this.ShowToast("Categoria inserida com sucesso!");
                        returnType = true;
                    }
                    else
                        this.ShowToast("Erro ao inserir categoria!");
                }

                this.CurrentOperation = FormOperation.Nenhum;
                this.CurrentCategory = null;
                this.Unblock();
                return returnType;
            } catch (Exception)
            {
                this.CurrentOperation = FormOperation.Nenhum;
                this.Unblock();
                return false;
            }
        }

        protected override async Task<bool> OnDelete(CategoryModel model)
        {
            try
            {
                if (CurrentCategory == null) return false;
                bool returnType = false;
                this.Block();
                CategoryModel deletedCategory = await categoryService.DeleteCategory(CurrentCategory.ID);
                if (deletedCategory != null)
                {
                    this.ShowToast("Categoria deletada com sucesso!");
                    if (!this.CategoryList.Remove(CurrentCategory)) await this.GetList();
                    this.CurrentCategory = null;
                    this.CurrentOperation = FormOperation.Nenhum;
                    this.Unblock();
                    returnType = true;
                }
                else
                {
                    this.ShowToast("Erro ao deletar categoria!");
                    returnType = false;
                }

                this.CurrentOperation = FormOperation.Nenhum;
                this.CurrentCategory = null;
                this.Unblock();
                return returnType;
            }
            catch (Exception)
            {
                this.ShowToast("Erro ao deletar categoria!");
                this.Unblock();
                this.CurrentOperation = FormOperation.Nenhum;
                this.CurrentCategory = null;
                return false;
            }
        }

        protected override bool OnCancel()
        {
            CurrentCategory = null;
            CurrentOperation = FormOperation.Nenhum;
            return true;
        }
    }
}
