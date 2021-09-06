using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AspAssignRolesRights
    {
        public Guid Id { get; set; }
        public Guid FkRightsId { get; set; }
        public Guid FkRolesId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual AspNetRights FkRights { get; set; }
        public virtual AspNetRoles FkRoles { get; set; }
    }
}
