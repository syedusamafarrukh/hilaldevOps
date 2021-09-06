using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Hilal.Common
{
    public static class FCMNotification
    {
        private static String APIKey = "AIzaSyAP_UNkO3cOXnxfpDztwArosgAtkh410Yo";
        private static String SenderId = "15612676007";
        private static String ServerKey = "AAAAA6KWh6c:APA91bFvEr3m_OxOYZpXaCVmNj7WSlcXTN9yhzx09bDSjx0muahbSt600acrR_9axPLMialrfzmtylBeV-Iqtn2-vE3ATeofbJFrLEyqW7Pw2SxCJWHhuZBGj_8qmn1T270w7W1ACbvj";
        public static void SentSilentNotify(string applicationId, string senderId, string deviceToken/*, object obj*/)
        {
            try
            {
                //string applicationID = applicationId;
                //string senderID = senderId;
                //string deviceId = deviceToken;
                
                string applicationID = ServerKey;
                string senderID = SenderId;
                string deviceId = deviceToken;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    //data = obj
                };
                var json = JsonConvert.SerializeObject(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderID));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Sentnotify(string applicationId, string senderId, string deviceToken, string body, string title, string sound, object obj,int Type)
        {
            try
            {
                //string applicationID = applicationId;
                //string senderID = senderId;
                //string deviceId = deviceToken;

                string applicationID = ServerKey;
                string senderID = SenderId;
                string deviceId = deviceToken;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = body,
                        title = title,
                        sound = sound,
                        Type = Type
                    },
                    Type = Type,
                    data = obj
                };
                var json = JsonConvert.SerializeObject(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderID));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
