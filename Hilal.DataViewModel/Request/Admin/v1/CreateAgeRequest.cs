using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateAgeRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; }

        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid? FK_CategoryId { get; set; }

        [JsonProperty(PropertyName = "Priority")]
        public int Priority { get; set; }

        [Required]
        [JsonProperty(PropertyName = "ageInformations")]
        public List<GenericDetailRequest> AgeInformations { get; set; }
        [JsonProperty(PropertyName = "subcategoryList")]
        public List<General<Guid>> subcategoryList { get; set; }
    }
}
