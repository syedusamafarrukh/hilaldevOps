using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Gender
    {
        public Gender()
        {
            AdminUsers = new HashSet<AdminUsers>();
            AppUserProfiles = new HashSet<AppUserProfiles>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual ICollection<AdminUsers> AdminUsers { get; set; }
        public virtual ICollection<AppUserProfiles> AppUserProfiles { get; set; }
    }
}
