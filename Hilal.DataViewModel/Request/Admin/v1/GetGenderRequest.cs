using Hilal.DataViewModel.Response.Admin.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class GetGenderRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "categoryId")]
        public Guid? CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AgeId")]
        public Guid? FK_AgeId { get; set; }
    }

    public class GetGenderByCategoryRequest : ListGeneralModel
    {
        [JsonProperty(PropertyName = "categoryId")]
        public List<Guid> CategoryId { get; set; }
        [JsonProperty(PropertyName = "FK_AgeId")]
        public List<Guid> FK_AgeId { get; set; }
    }
}
