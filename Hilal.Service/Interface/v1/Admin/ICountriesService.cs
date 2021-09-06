using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface ICountriesService
    {
        GetCountriesResponse GetCountries(ListGeneralModel pCountries);
        Task<bool> SaveCountries(CreateCountriesRequest CountriesRequest, Guid userId);
        CreateCountriesRequest GetEditCountries(Guid id, Guid userId);
        Task<bool> ControllCountriesActivation(Guid CountriesId, bool activation, Guid userId);
        List<GetCountries> GetCountriesList(int LanguageId);
    }
}
