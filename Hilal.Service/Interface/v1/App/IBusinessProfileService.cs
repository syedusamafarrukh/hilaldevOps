using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.DataViewModel.Response.App.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.App
{
    public interface IBusinessProfileService
    {
        GetUserBusinessProfileResponse GetBusinessProfileService(GetBusinessProfileRequest BusinessProfile, Guid? userId);
        GetUserBusinessProfileListResponse GetBusinessServices(GetBusinessProfileRequest BusinessProfile, Guid? userId);
        Task<GetUserBusinessProfileResponse> GetBusinessProfileServiceSP(GetBusinessProfileRequest BusinessProfile, Guid? userId);
        Task<GetUserBusinessProfileListResponse> GetBusinessServicesSP(GetBusinessProfileRequest BusinessProfile, Guid? userId);
        GetServicesDetail GetBusinessServicesDetail(Guid Id, int LanguageId, Guid? userId);
        Task<bool> SaveBusinessProfile(CreateAppUserBusinessProfileRequest BusinessProfileRequest, Guid userId);
        CreateAppUserBusinessProfileRequest GetEditBusinessProfile(Guid id);
        GetEditUserBusinessProfileRequest GetEditBusinessProfileApp(Guid id, long LanguageId);
        Task<bool> ControllBusinessProfileActivation(Guid BusinessProfileId, bool activation, Guid userId);
        GetBusinessProfile GetBusinessProfileServiceByUserId(Guid appUserId, long LanguageId);
        Task<bool> ControllBusinessProfileStatus(Guid BusinessProfileId, int StatusId, Guid userId, string Comments);
        Task<bool> ControllBusinessProfileStatusApp(Guid BusinessProfileId, int StatusId, Guid userId);
    }
}
