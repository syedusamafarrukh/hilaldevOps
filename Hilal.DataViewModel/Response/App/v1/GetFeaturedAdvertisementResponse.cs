using Hilal.DataViewModel.Response.Admin.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetFeaturedAdvertisementResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "ItemList")]
        public List<GetFeaturedAdvertisementList> ItemList { get; set; }
    }

    public class GetFeaturedAdvertisementList
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_AdvertisementId")]
        public Guid FK_AdvertisementId { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Priority")]
        public int Priority { get; set; }
        [JsonProperty(PropertyName = "RefId")]
        public string RefId { get; set; }
    }
}
