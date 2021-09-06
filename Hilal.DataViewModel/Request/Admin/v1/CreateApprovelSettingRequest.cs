using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateApprovelSettingRequest
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "ApprovelType")]
        public int ApprovelType { get; set; }
        [Required]
        [JsonProperty(PropertyName = "CategoryType")]
        public int CategoryType { get; set; }
    }
}
