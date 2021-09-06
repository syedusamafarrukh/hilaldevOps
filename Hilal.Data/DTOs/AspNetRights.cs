using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AspNetRights
    {
        public AspNetRights()
        {
            AspAssignRolesRights = new HashSet<AspAssignRolesRights>();
            InverseFkSelfNavigation = new HashSet<AspNetRights>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public long? Grade { get; set; }
        public Guid? FkSelf { get; set; }
        public string Logo { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        public virtual AspNetRights FkSelfNavigation { get; set; }
        public virtual ICollection<AspAssignRolesRights> AspAssignRolesRights { get; set; }
        public virtual ICollection<AspNetRights> InverseFkSelfNavigation { get; set; }
    }
}
