using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IAgeService
    {
        GetAgeResponse GetAge(GetAgeRequest page);
        Task<bool> SaveAge(CreateAgeRequest AgeRequest, Guid userId);
        List<GetAgesLsit> GetAgeByCategories(GetAgeByCategoryRequest page);
        CreateAgeRequest GetEditAge(Guid id, Guid userId);
        Task<bool> ControllAgeActivation(Guid AgeId, bool activation, Guid userId);
        List<GetAgesLsit> GetAgeList(GetAgeRequest page);
    }
}
