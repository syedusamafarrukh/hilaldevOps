using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetAgeResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetAge> ItemList { get; set; } = new List<GetAge>();
    }

    public class GetAge
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public General<Guid?> FK_CategoryId { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public List<General<Guid?>> FK_SubCategoryId { get; set; }
    }

    public class GetAgesLsit
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
    }
}
