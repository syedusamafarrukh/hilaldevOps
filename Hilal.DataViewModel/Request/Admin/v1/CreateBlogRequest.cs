using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateBlogRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "HeaderImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "ThumbnilUrl")]
        public string ThumbnilUrl { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }
}
