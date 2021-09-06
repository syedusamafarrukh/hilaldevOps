using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Hilal.Common
{
    public class SentSMS
    {
        private String apIkey = "5a56e972b6a7192685a716bce053c4d8";
        private String secreatkey = "0f182846e9e92f67b432ce93e247f5d9";
        public string ApiKey = "AIzaSyAczDhhs7SJZ331MGrR77Kl-dm85Dzwx4Y";
        public string client_id = "info@douxl.com";
        public string Token = "a861b06cf5c1e4e6a8e18c631bdef194ee656f92";
        public async Task<bool> SendSMS(string to, string text)
        {
            using (var httpClient = new HttpClient())
            {
                var sso = new { action = "sendsms", user = "h72fa73p" , password = "4dkZ3yJW", from = "AlHalal", to = to, text = text };
                //var json = JsonConvert.SerializeObject();
                var query = @"https://api.smsglobal.com/http-api.php?" + $"action={sso.action}&user={sso.user}&password={sso.password}&from={sso.from}&to={sso.to}&text={HttpUtility.UrlEncode(sso.text)}";
                var response = httpClient.PostAsync(query, null).Result;
                var result = await response.Content.ReadAsStringAsync();
                return true;
            }
        }
        public string SendSmsToUser(string textMessage, string to, string twilioAccountSID, string TwilioAuthToken, string TwilioFromNumber)
        {
            try
            {
                TwilioClient.Init(twilioAccountSID, TwilioAuthToken);

                var message = MessageResource.Create(
                    body: textMessage,
                    from: new Twilio.Types.PhoneNumber(TwilioFromNumber),
                    to: new Twilio.Types.PhoneNumber(to)
                );

                return message.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
