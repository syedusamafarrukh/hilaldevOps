using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetChatStreamResponse : GeneralGetList
    {
        [JsonProperty(PropertyName = "ChatList")]
        public List<ChatStreamViewModel> ChatList { get; set; }
        [JsonProperty(PropertyName = "UserId")]
        public Guid UserId { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "UserDP")]
        public FileUrlResponce UserDP { get; set; } = new FileUrlResponce();
    }

    public class ChatStreamViewModel
    {
        [JsonProperty(PropertyName = "ThreadId")]
        public Guid? ThreadId { get; set; }
        [JsonProperty(PropertyName = "FK_SenderId")]
        public Guid FK_SenderId { get; set; }
        [JsonProperty(PropertyName = "FK_SenderName")]
        public string FK_SenderName { get; set; }
        [JsonProperty(PropertyName = "FK_SenderDp")]
        public FileUrlResponce FK_SenderDp { get; set; } = new FileUrlResponce();
        //[JsonProperty(PropertyName = "FK_ReceiverId")]
        //public Guid FK_ReceiverId { get; set; }
        //[JsonProperty(PropertyName = "FK_ReceiverName")]
        //public string FK_ReceiverName { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "FK_ServiceId")]
        public Guid? FK_ServiceId { get; set; }
        [JsonProperty(PropertyName = "MessageText")]
        public string MessageText { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty(PropertyName = "IsRead")]
        public bool IsRead { get; set; }
    }
}
