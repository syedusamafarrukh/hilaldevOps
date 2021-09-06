using Hilal.DataViewModel.Response.Admin.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class GetCategoriesRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "categoryId")]
        public Guid? CategoryId { get; set; }
        [JsonProperty(PropertyName = "SubCategoryList")]
        public bool SubCategoryList { get; set; }
        [JsonProperty(PropertyName = "FkCategoryType")]
        public int? FkCategoryType { get; set; }
    }

    public class GetSubCategoriesRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "categoryId")]
        public List<Guid> CategoryId { get; set; }
        [JsonProperty(PropertyName = "FkCategoryType")]
        public int? FkCategoryType { get; set; }
    }
}
