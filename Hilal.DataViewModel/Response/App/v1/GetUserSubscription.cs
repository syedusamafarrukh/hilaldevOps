using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hilal.DataViewModel.Response.App.v1
{
    public class GetUserSubscription
    {
        [JsonProperty(PropertyName = "FK_SubscribedPlanId")]
        public Guid FK_SubscribedPlanId { get; set; }
        [JsonProperty(PropertyName = "PlanName")]
        public string PlanName { get; set; }
    }
}
