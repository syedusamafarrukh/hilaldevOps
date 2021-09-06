using Hilal.DataViewModel.Response.Admin.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class GetBreedRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "BreedId")]
        public Guid? BreedId { get; set; }
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public Guid? FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public Guid? FK_SubCategoryId { get; set; }
    }

    public class GetBreedByCategoryRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "FK_CategoryId")]
        public List<Guid> FK_CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_SubCategoryId")]
        public List<Guid> FK_SubCategoryId { get; set; }
    }
}
