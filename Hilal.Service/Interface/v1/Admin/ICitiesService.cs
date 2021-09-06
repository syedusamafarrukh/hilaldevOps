using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface ICitiesService
    {
        GetCitiesResponse GetCities(GetCityRequest pCities);
        Task<bool> SaveCities(CreateCitiesRequest CitiesRequest, Guid userId);
        CreateCitiesRequest GetEditCities(Guid id, Guid userId);
        Task<bool> ControllCitiesActivation(Guid CitiesId, bool activation, Guid userId);
        List<GetCitiesList> GetCitiesList(Guid? countryId, int LanguageId);
    }
}
