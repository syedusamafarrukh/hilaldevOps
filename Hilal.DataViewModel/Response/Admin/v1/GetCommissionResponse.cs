using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetCommissionResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "itemList")]
        public List<GetCommission> ItemList { get; set; } = new List<GetCommission>();
    }

    public class GetCommission
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public General<Guid?> FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Percentage")]
        public double Percentage { get; set; }
        [JsonProperty(PropertyName = "StartRange")]
        public double StartRange { get; set; }
        [JsonProperty(PropertyName = "EndRange")]
        public double EndRange { get; set; }
        [JsonProperty(PropertyName = "StartRange")]
        public string DisplayRange { get; set; }
        [JsonProperty(PropertyName = "DisplayPercentage")]
        public string DisplayPercentage { get; set; }
    }
}
