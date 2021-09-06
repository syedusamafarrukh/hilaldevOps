using Hilal.DataViewModel.Request.App.v1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Hilal.Common
{
    public static class NetworkGlobal
    {
        private static string APIKey = "AIzaSyAP_UNkO3cOXnxfpDztwArosgAtkh410Yo";
        private static string devSubLink = "https://alhilal.douxl.dev/#/subscription-payment-status?PlanId=";
        private static string devadpaymentLink = "https://alhilal.douxl.dev/#/ad-payment-status?AdvertisementId=";
        private static string APIKeyAccess = "AIzaSyAP_UNkO3cOXnxfpDztwArosgAtkh410Yo";
        private static string outletRef = "b73fe607-ec4a-42c4-8f32-3feae9b76fd8";
        private static string outletId = "17257210-aeb0-4084-aaa2-ea7380005d2f";
        private static string Key = "YWRkNGQ0ZDItYTlhNC00NjFhLThlNTktMDYyN2QyZTk3MDRiOjllZTA4ZGMxLWJkN2EtNDJlNy05MjYwLWRjOWM5ZjUzNWUxZA==";
        private static string url = "https://api-gateway.sandbox.ngenius-payments.com/transactions/outlets/["+ outletId +"]/payment/card";

        public static string GetAccessToken()
        {
            try
            {
                while (true)
                {
                    //var client = new RestClient("https://api-gateway.sandbox.ngenius-payments.com/identity/auth/access-token");
                    var client = new RestClient("https://api-gateway.ngenius-payments.com/identity/auth/access-token");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Accept", "application/vnd.ni-identity.v1+json");
                    request.AddHeader("Authorization", "Basic " + "YWRkNGQ0ZDItYTlhNC00NjFhLThlNTktMDYyN2QyZTk3MDRiOjllZTA4ZGMxLWJkN2EtNDJlNy05MjYwLWRjOWM5ZjUzNWUxZA==");
                    request.AddHeader("Content-Type", "application/vnd.ni-identity.v1+json");
                    //request.AddParameter("application/vnd.ni-identity.v1+json", "{\"realmName\":\"ni\"}", ParameterType.RequestBody);
                    //request.AddParameter("application/vnd.ni-identity.v1+json", "{\"userName\":\"bader6111800@hotmail.com\"}", ParameterType.RequestBody);
                    //request.AddParameter("application/vnd.ni-identity.v1+json", "{\"password\":\"Ss19691969@\"}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var res = response.Content;
                    var data = "";
                    if (response.Content.Contains("access_token"))
                    {
                        data = JObject.Parse(res)["access_token"].ToString();
                        APIKeyAccess = data;
                    }
                    if (!String.IsNullOrEmpty(data))
                    {
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string SinglePaymentMethod(CreatePaymentOrderRequest createPaymentOrderRequest)
        {
            try
            {
                var accessToken = GetAccessToken();
                int amount = (int) Math.Round(createPaymentOrderRequest.Amount);
                //var client = new RestClient("https://api-gateway.sandbox.ngenius-payments.com/transactions/outlets/"+outletId+"/orders");
                var client = new RestClient("https://api-gateway.ngenius-payments.com/transactions/outlets/" + outletId+"/orders");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/vnd.ni-payment.v2+json");
                request.AddHeader("Content-Type", "application/vnd.ni-payment.v2+json");
                request.AddHeader("Authorization", "Bearer "+ accessToken);
                request.AddParameter("application/vnd.ni-payment.v2+json", "{\"action\":\"SALE\",\"amount\":{\"currencyCode\":\"AED\",\"value\":"+ amount + "},\"emailAddress\":\"" + createPaymentOrderRequest.EmailAddress + "\",\"merchantAttributes\":{\"redirectUrl\":\""+ devSubLink + createPaymentOrderRequest.planId+ "\",\"skipConfirmationPage\":true}, \"billingAddress\":{\"firstName\":\"" + createPaymentOrderRequest.firstName + "\",\"lastName\":\"" + createPaymentOrderRequest.lastName + "\",\"address1\":\"" + createPaymentOrderRequest.Address + "\",\"city\":\"" + createPaymentOrderRequest.city + "\",\"countryCode\":\"UAE\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var paymentRef = "";
                if (response.Content.Contains("_links"))
                {
                    if (response.Content.Contains("payment"))
                    {
                        var parse = JObject.Parse(response.Content);
                        paymentRef = parse["_links"]["payment"]["href"].ToString(); 
                    }
                }
                //checkOrderStatus("70ecedf3-f19e-4c4c-9927-c364dec22933");
                return paymentRef;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string commissionPaymentMethod(CreateAdCommisionRequest createPaymentOrderRequest)
        {
            try
            {
                var accessToken = GetAccessToken();
                int amount = (int)Math.Round(createPaymentOrderRequest.Amount);
                //var client = new RestClient("https://api-gateway.sandbox.ngenius-payments.com/transactions/outlets/" + outletId + "/orders");
                var client = new RestClient("https://api-gateway.ngenius-payments.com/transactions/outlets/" + outletId + "/orders");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/vnd.ni-payment.v2+json");
                request.AddHeader("Content-Type", "application/vnd.ni-payment.v2+json");
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddParameter("application/vnd.ni-payment.v2+json", "{\"action\":\"SALE\",\"amount\":{\"currencyCode\":\"AED\",\"value\":" + amount + "},\"emailAddress\":\"" + createPaymentOrderRequest.EmailAddress + "\",\"merchantAttributes\":{\"redirectUrl\":\"" + devadpaymentLink + createPaymentOrderRequest.AdvertisementId + "\",\"skipConfirmationPage\":true}, \"billingAddress\":{\"firstName\":\"" + createPaymentOrderRequest.firstName + "\",\"lastName\":\"" + createPaymentOrderRequest.lastName + "\",\"address1\":\"" + createPaymentOrderRequest.Address + "\",\"city\":\"" + createPaymentOrderRequest.city + "\",\"countryCode\":\"UAE\"}}", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var paymentRef = "";
                if (response.Content.Contains("_links"))
                {
                    if (response.Content.Contains("payment"))
                    {
                        var parse = JObject.Parse(response.Content);
                        paymentRef = parse["_links"]["payment"]["href"].ToString();
                    }
                }
                //checkOrderStatus("70ecedf3-f19e-4c4c-9927-c364dec22933");
                return paymentRef;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string checkOrderStatus(string orderRef)
        {
            try
            {
                GetAccessToken();
                //var client1 = new RestClient("https://api-gateway.sandbox.ngenius-payments.com/transactions/outlets/" + outletId + "/orders/"+ orderRef);
                var client1 = new RestClient("https://api-gateway.ngenius-payments.com/transactions/outlets/" + outletId + "/orders/"+ orderRef);
                var request1 = new RestRequest(Method.GET);
                request1.AddHeader("Accept", "application/vnd.ni-payment.v2+json");
                request1.AddHeader("Authorization", "Bearer " + APIKeyAccess);
                IRestResponse response1 = client1.Execute(request1);
                var paymentRef = "";
                if (response1.Content.Contains("_embedded"))
                {
                    if (response1.Content.Contains("payment"))
                    {
                        var parse = JObject.Parse(response1.Content);
                        paymentRef = parse["_embedded"]["payment"][0]["state"].ToString();
                    }
                }
                return paymentRef;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
