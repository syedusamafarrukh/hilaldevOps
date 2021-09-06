using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateCitiesRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }
        [JsonProperty(PropertyName = "FkCountry")]
        public Guid? FkCountry { get; set; }

        [Required]
        [JsonProperty(PropertyName = "CitiesInformations")]
        public List<GenericDetailRequest> CitiesInformations { get; set; }
    }
}
