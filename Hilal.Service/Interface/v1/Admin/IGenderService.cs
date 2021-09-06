using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IGenderService
    {
        GetGenderResponse GetGender(GetGenderRequest Gender);
        List<GetGenderList> GetGenderByCategory(GetGenderByCategoryRequest Gender);
        Task<bool> SaveGender(CreateGenderRequest GenderRequest, Guid userId);
        CreateGenderRequest GetEditGender(Guid id, Guid userId);
        Task<bool> ControllGenderActivation(Guid GenderId, bool activation, Guid userId);
        List<GetGenderList> GetGenderList(GetGenderRequest Gender);
    }
}
