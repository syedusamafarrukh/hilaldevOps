using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface ICategoryService
    {
        GetCategoryResponse GetCategories(GetCategoriesRequest page);
        List<GetCategory> GetSubCategoriesByCategoryList(GetSubCategoriesRequest page);
        Task<bool> SaveCategory(CreateCategoryRequest categoryRequest, Guid userId);
        CreateCategoryRequest GetEditCategory(Guid id, Guid userId);
        Task<bool> ControllCategoryActivation(Guid categoryId, bool activation, Guid userId);
        List<GetCategory> GetCategoriesList(GetCategoriesRequest page);
    }
}
