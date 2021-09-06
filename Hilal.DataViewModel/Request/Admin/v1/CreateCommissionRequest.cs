using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateCommissionRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "Percentage")]
        public double Percentage { get; set; }
        [JsonProperty(PropertyName = "StartRange")]
        public double StartRange { get; set; }
        [JsonProperty(PropertyName = "EndRange")]
        public double EndRange { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid? FK_CategoryId { get; set; }
        [Required]
        [JsonProperty(PropertyName = "CommissionInformations")]
        public List<CommissionDetailRequest> CommissionInformations { get; set; }
    }

    public class CommissionDetailRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "fkmasterId")]
        public Guid FKMasterId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "languageId")]
        public long LanguageId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "DisplayRange")]
        public string DisplayRange { get; set; }
        [JsonProperty(PropertyName = "DisplayPercentage")]
        public string DisplayPercentage { get; set; }
    }
}
