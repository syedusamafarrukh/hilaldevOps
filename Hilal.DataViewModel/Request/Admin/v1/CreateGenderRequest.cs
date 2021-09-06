using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateGenderRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "GenderInformations")]
        public List<GenericDetailRequest> GenderInformations { get; set; }
    }

    public class CreateGenderAges
    {
        [JsonProperty(PropertyName = "FK_GenderId")]
        public Guid? FK_GenderId { get; set; }

        [JsonProperty(PropertyName = "FK_AgeId")]
        public Guid FK_AgeId { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid FK_CategoryId { get; set; }
    }
}
