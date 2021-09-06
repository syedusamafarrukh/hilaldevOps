using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Response.App.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App
{
    public class GetChatThreadResponse : GeneralGetList
    {
        [JsonProperty(PropertyName = "ThreadList")]
        public List<ThreadViewModel> ThreadList { get; set; }
    }

    public class ThreadViewModel
    {
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "FK_SenderId")]
        public Guid FK_SenderId { get; set; }
        [JsonProperty(PropertyName = "FK_SenderName")]
        public string FK_SenderName { get; set; }
        [JsonProperty(PropertyName = "FK_SenderDp")]
        public FileUrlResponce FK_SenderDp { get; set; } = new FileUrlResponce();
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "FK_ServiceId")]
        public Guid? FK_ServiceId { get; set; }
        [JsonProperty(PropertyName = "MessageText")]
        public string MessageText { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty(PropertyName = "UnReadCount")]
        public int UnReadCount { get; set; }
    }
}
