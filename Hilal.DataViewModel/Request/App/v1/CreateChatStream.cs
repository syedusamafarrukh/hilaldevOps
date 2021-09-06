using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateChatStream
    {
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "FK_SenderId")]
        public Guid FK_SenderId { get; set; }
        [JsonProperty(PropertyName = "FK_ReceiverId")]
        public Guid FK_ReceiverId { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "FK_ServiceId")]
        public Guid? FK_ServiceId { get; set; }
        [JsonProperty(PropertyName = "MessageText")]
        public string MessageText { get; set; }
    }
}
