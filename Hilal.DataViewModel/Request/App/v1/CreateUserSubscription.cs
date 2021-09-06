using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Request.App.v1
{
    public class CreateUserSubscription
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "FK_UserId")]
        public Guid FK_UserId { get; set; }
        [JsonProperty(PropertyName = "FK_SubscribedPlanId")]
        public Guid FK_SubscribedPlanId { get; set; }
    }
}
