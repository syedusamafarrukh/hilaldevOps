using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class RoleRights
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid RightId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual Rights Right { get; set; }
        public virtual Roles Role { get; set; }
    }
}
