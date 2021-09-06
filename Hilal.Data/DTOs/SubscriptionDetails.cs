using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class SubscriptionDetails
    {
        public Guid Id { get; set; }
        public Guid FkSubscriptionId { get; set; }
        public string Name { get; set; }
        public string DisplayValidityDays { get; set; }
        public long FkLanguageId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual Languages FkLanguage { get; set; }
        public virtual Subsciption FkSubscription { get; set; }
    }
}
