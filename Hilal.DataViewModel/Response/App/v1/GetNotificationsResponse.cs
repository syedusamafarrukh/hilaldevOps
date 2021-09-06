using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetNotificationsResponse : GeneralGetList
    {
        [JsonProperty(PropertyName = "ItemList")] 
        public List<GetNotificationsViewModels> ItemList { get; set; }
        
        [JsonProperty(PropertyName = "UnReadCount")] 
        public int UnReadCount { get; set; }
    }

    public class GetNotificationsViewModels
    {
        [JsonProperty(PropertyName = "AdvertisementId")]
        public Guid? AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "ServiceId")]
        public Guid? ServiceId { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "ReceiverId")]
        public Guid? ReceiverId { get; set; }
        [JsonProperty(PropertyName = "BodyText")]
        public string BodyText { get; set; }
        [JsonProperty(PropertyName = "IsSeen")]
        public bool IsSeen { get; set; }
        [JsonProperty(PropertyName = "CreatedDate")]
        public DateTime CreatedDate { get; set; }
    }
}
