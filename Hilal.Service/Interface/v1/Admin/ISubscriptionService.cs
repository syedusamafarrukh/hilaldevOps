using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Request.App.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface ISubscriptionService
    {
        GetSubscriptionResponse GetSubscription(ListGeneralModel Subscription);
        Task<bool> SaveSubscription(CreateSubscriptionRequest SubscriptionRequest, Guid userId);
        CreateSubscriptionRequest GetEditSubscription(Guid id, Guid userId);
        Task<bool> ControllSubscriptionActivation(Guid SubscriptionId, bool activation, Guid userId);
        List<GetSubscriptionList> GetSubscriptionList(int LanguageId);
        Task<string> CreateOrderRequest(CreatePaymentOrderRequest createPaymentOrderRequest, Guid userId);
        Task<bool> createSubscrption(string orderRef, Guid planId, Guid userId);
        bool checkSubscription(Guid userId);
    }
}
