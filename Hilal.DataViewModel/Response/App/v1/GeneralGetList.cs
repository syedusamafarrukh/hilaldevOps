using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GeneralGetList
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; } = "";

        [JsonProperty(PropertyName = "skip")]
        public int Skip { get; set; }

        [JsonProperty(PropertyName = "take")]
        public int Take { get; set; }
        [JsonProperty(PropertyName = "totalRecords")]
        public int? TotalRecords { get; set; }
        [JsonProperty(PropertyName = "LanguageId")]
        public int? LanguageId { get; set; }
    }
}
