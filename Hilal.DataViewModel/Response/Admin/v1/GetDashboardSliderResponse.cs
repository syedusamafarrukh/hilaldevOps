using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Hilal.DataViewModel.Request;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetDashboardSliderResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetGetDashboardSliderResponse> ItemList { get; set; } = new List<GetGetDashboardSliderResponse>();
    }

    public class GetGetDashboardSliderResponse
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThubnilUrl")]
        public string ThubnilUrl { get; set; }
    }
    
    public class GetGetDashboardSliderResponseList
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThubnilUrl")]
        public string ThubnilUrl { get; set; }
    }
}
