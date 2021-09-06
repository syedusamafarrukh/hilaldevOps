using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AspNetUserRoles
    {
        public Guid Id { get; set; }
        public Guid FkUserId { get; set; }
        public Guid FkRoleId { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual AspNetRoles FkRole { get; set; }
        public virtual AspNetUsers FkUser { get; set; }
    }
}
