using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AppUserSubscription
    {
        public Guid Id { get; set; }
        public Guid FkUserId { get; set; }
        public Guid FkSubscribedPlanId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual Subsciption FkSubscribedPlan { get; set; }
        public virtual AppUsers FkUser { get; set; }
    }
}
