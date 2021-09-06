using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.DataViewModel.Response.App.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IFeaturedAdvertisementService
    {
        GetFeaturedAdvertisementResponse GetFeaturedAdvertisement(ListGeneralModel page);
        Task<bool> SaveFeaturedAdvertisement(CreateFeaturedAdvertisementRequest FeaturedAdvertisementRequest, Guid userId);
        CreateFeaturedAdvertisementRequest GetEditFeaturedAdvertisement(Guid id);
        Task<bool> ControllFeaturedAdvertisementActivation(Guid FeaturedAdvertisementId, bool activation, Guid userId);
        List<GetAdvertisementList> GetFeaturedAdvertisementList(int LanguageId);
    }
}
