using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Roles
    {
        public Roles()
        {
            AdminUserRoles = new HashSet<AdminUserRoles>();
            RoleRights = new HashSet<RoleRights>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool? IsSuperAdmin { get; set; }

        public virtual ICollection<AdminUserRoles> AdminUserRoles { get; set; }
        public virtual ICollection<RoleRights> RoleRights { get; set; }
    }
}
