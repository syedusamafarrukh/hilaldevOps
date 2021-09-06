using Hilal.DataViewModel.Request.Admin.v1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.Admin.v1
{
    public class GetUsersResponse : ListGeneralModel
    {
        [JsonProperty(PropertyName = "users")]
        public List<CreateUserRequest> Users { get; set; } = new List<CreateUserRequest>();
    }
}
