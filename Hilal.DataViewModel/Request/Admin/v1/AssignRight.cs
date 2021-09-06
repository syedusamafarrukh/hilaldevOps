using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hilal.DataViewModel.Request.Admin.v1
{
    public class AssignRight
    {
        [Required]
        [JsonProperty(PropertyName = "roleId")]
        public Guid RoleId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "rightIds")]
        public List<Guid> RightIds { get; set; }
    }
}
