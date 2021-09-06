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
    public interface IAdvertisementService
    {
        List<GetAdvertisementList> GetAdvertisement(GetAdvertisementRequest advertisementRequest, Guid? userId);
        GetAppAdvertisementResponse GetFiltersAdvertisements(GetFiltersResquest advertisementRequest);
        Task<bool> ControllAdvertisementStatusfromApp(Guid AdvertisementId, int StatusId, Guid userId);
        GetAdvertisemetDetail GetAdvertisementDetail(Guid Id, int LanguageId, Guid? userId);
        Task<Guid> SaveAdvertisement(CreateAdvertisementRequest advertisementRequest, Guid userId);
        Task<bool> CreateAdvertisementCommission(CreateCommisionPaymentRequest CreateAdvertisementCommission, Guid userId);
        CreateAdvertisementRequest GetEditAdvertisement(Guid id);
        getEditAdvertisementRequest GetEditAdvertisementforAPP(Guid id, long LanguageId);
        Task<bool> ControllAdvertisementActivation(Guid AdvertisementId, bool activation, Guid userId);
        Task<bool> ControllAdvertisementStatus(Guid AdvertisementId, int StatusId, Guid userId, String Comments);
        GetAppAdvertisementResponse GetAdvertisementServicePList(GetAdvertisementRequest advertisementRequest, Guid? userId, int adminType);
        Task<GetAppAdvertisementResponse> GetAdvertisementService(GetAdvertisementRequest advertisementRequest, Guid? userId, int adminType);
        Task<bool> SaveBookMarksAdvertisement(CreateBookMarksAdvertisementRequest advertisementRequest, Guid userId);
        List<GetAdvertisementList> GetBookmarksAdvertisementByUserId(int LanguageId, Guid userId);
        Task<bool> AddUserSubscription(CreateUserSubscription CreateUserSubscription, Guid userId);
        GetSubscriptionList GetUserSubscribedDetail(Guid userId, int LanguageId);
        GetAdminNotificationResponse GetAdminNotifications(ListGeneralModel page);
        Task<string> CreateOrderRequest(CreateAdCommisionRequest createPaymentOrderRequest, Guid userId);
    }
}
