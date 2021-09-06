using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Subsciption
    {
        public Subsciption()
        {
            AppUserSubscription = new HashSet<AppUserSubscription>();
            SubscriptionDetails = new HashSet<SubscriptionDetails>();
        }

        public Guid Id { get; set; }
        public int ValidityDays { get; set; }
        public int? ValidityPosts { get; set; }
        public decimal Amount { get; set; }
        public bool? IsDisplayed { get; set; }
        public bool? IsBlock { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
        public decimal? NewAmount { get; set; }

        public virtual ICollection<AppUserSubscription> AppUserSubscription { get; set; }
        public virtual ICollection<SubscriptionDetails> SubscriptionDetails { get; set; }
    }
}
