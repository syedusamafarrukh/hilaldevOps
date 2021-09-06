using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IBlogService
    {
        GetBlogResponse GetBlog(ListGeneralModel pBlog);
        Task<bool> SaveBlog(CreateBlogRequest BlogRequest, Guid userId);
        CreateBlogRequest GetEditBlog(Guid id, Guid userId);
        Task<bool> ControllBlogActivation(Guid BlogId, bool activation, Guid userId);
        List<BlogViewModel> GetBlogList(Guid? countryId, int LanguageId);
    }
}
