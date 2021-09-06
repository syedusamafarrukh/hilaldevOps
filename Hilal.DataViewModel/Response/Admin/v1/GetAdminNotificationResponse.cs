using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetAdminNotificationResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetNotificationsViewModel> ItemList { get; set; } = new List<GetNotificationsViewModel>();
        [JsonProperty(PropertyName = "IsSeenCount")]
        public int IsSeenCount { get; set; } = 0;

    }


    public class GetNotificationsViewModel 
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid? FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "FK_ServiceId")]
        public Guid? FK_ServiceId { get; set; }
        [JsonProperty(PropertyName = "BodyText")]
        public string BodyText { get; set; }
        [JsonProperty(PropertyName = "IsSeen")]
        public bool IsSeen { get; set; }
        
    }
}
