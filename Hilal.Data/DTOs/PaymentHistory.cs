using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class PaymentHistory
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Guid AppUserId { get; set; }
        public bool IsSubscription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPaymentDone { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual AppUsers AppUser { get; set; }
    }
}
