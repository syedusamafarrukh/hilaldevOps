using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspAssignRolesRights = new HashSet<AspAssignRolesRights>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual ICollection<AspAssignRolesRights> AspAssignRolesRights { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
