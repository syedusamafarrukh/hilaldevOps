using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class UserCards
    {
        public Guid Id { get; set; }
        public Guid FkAppUserId { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Csv { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual AdminUsers FkAppUser { get; set; }
    }
}
