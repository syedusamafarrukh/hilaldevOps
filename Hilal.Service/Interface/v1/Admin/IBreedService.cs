using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IBreedService
    {
        GetBreedResponse GetBreed(GetBreedRequest page);
        List<GetBreedList> GetBreedByCategoryList(GetBreedByCategoryRequest page);
        Task<bool> SaveBreed(CreateBreedRequest breedRequest, Guid userId);
        CreateBreedRequest GetEditBreed(Guid id, Guid userId);
        Task<bool> ControllBreedActivation(Guid breedId, bool activation, Guid userId);
        List<GetBreedList> GetBreedList(GetBreedRequest page);
    }
}
