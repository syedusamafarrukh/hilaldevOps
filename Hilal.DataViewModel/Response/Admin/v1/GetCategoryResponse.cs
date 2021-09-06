using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetCategoryResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetCategory> ItemList { get; set; } = new List<GetCategory>();
    }

    public class GetCategory
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "IsSubCategory")]
        public bool IsSubCategory { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();

        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }
        [JsonProperty(PropertyName = "FkCategoryType")]
        public int? FkCategoryType { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public General<Guid?> FK_CategoryId { get; set; }
    }
}
