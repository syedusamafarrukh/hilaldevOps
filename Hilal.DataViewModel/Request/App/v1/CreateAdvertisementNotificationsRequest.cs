using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateAdvertisementNotificationsRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "ReceiverId")]
        public Guid? ReceiverId { get; set; }
        [JsonProperty(PropertyName = "BodyText")]
        public string BodyText { get; set; }
        [JsonProperty(PropertyName = "DeviceToken")]
        public string DeviceToken { get; set; }
    }
}
