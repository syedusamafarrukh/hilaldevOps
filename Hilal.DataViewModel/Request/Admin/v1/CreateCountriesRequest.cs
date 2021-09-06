using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateCountriesRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "CountriesInformations")]
        public List<GenericDetailRequest> CountriesInformations { get; set; }
    }
}
