using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateDashboardSliderRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "SliderDetails")]
        public List<CreateDashboardSliderDetailsRequest> SliderDetails { get; set; }
        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "ThubnilUrl")]
        public string ThubnilUrl { get; set; }
    }
    public class CreateDashboardSliderDetailsRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_DashboardSliderId")]
        public Guid FK_DashboardSliderId { get; set; }
        [JsonProperty(PropertyName = "FK_LanguageId")]
        public long FK_LanguageId { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }
}
