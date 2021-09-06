using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateBreedRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "FK_SelfBreedId")]
        public Guid? FK_SelfBreedId { get; set; }

        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; }

        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid? FK_CategoryId { get; set; }

        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();

        [Required]
        [JsonProperty(PropertyName = "breedInformations")]
        public List<GenericDetailRequest> BreedInformations { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public int? Priority { get; set; }

        [JsonProperty(PropertyName = "subcategoryList")]
        public List<General<Guid>> subcategoryList { get; set; }
    }
}
