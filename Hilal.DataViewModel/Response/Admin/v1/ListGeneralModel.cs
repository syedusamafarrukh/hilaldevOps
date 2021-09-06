using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class ListGeneralModel
    {
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "totalRecords")]
        public int TotalRecords { get; set; }

        [JsonProperty(PropertyName = "sortIndex")]
        public int? SortIndex { get; set; } = -1;

        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "sortBy")]
        public string SortBy { get; set; }
        [JsonProperty(PropertyName = "LanguageId")]
        public long? LanguageId { get; set; }
    }
}
