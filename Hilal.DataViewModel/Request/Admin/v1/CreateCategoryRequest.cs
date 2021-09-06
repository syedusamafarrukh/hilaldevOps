using Hilal.DataViewModel.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class CreateCategoryRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? Id { get; set; }

        [JsonProperty(PropertyName = "categoryId")]
        public Guid? CategoryId { get; set; }
        [JsonProperty(PropertyName = "IsSubCategory")]
        public bool IsSubCategory { get; set; }

        [JsonProperty(PropertyName = "image")]
        public FileUrlResponce Image { get; set; } = new FileUrlResponce();

        [Required]
        [JsonProperty(PropertyName = "categoryInformations")]
        public List<GenericDetailRequest> CategoryInformations { get; set; }

        [Required]
        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }

        [Required]
        [JsonProperty(PropertyName = "FkCategoryType")]
        public int? FkCategoryType { get; set; }
    }
}
