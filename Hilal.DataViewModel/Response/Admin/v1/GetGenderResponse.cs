using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetGenderResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetGender> ItemList { get; set; } = new List<GetGender>();
    }

    public class GetGender
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "FKCategoryId")]
        public General<Guid?> FKCategoryId { get; set; }
        [JsonProperty(PropertyName = "AgeId")]
        public General<Guid?> AgeId { get; set; }
    }

    public class GetGenderList
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

    }
}
