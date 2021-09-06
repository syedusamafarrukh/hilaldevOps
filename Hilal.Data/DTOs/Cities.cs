using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Cities
    {
        public Cities()
        {
            Advertisement = new HashSet<Advertisement>();
            AppUserProfiles = new HashSet<AppUserProfiles>();
            Citydetails = new HashSet<Citydetails>();
            UserBusinessProfile = new HashSet<UserBusinessProfile>();
        }

        public Guid Id { get; set; }
        public Guid? FkCountry { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }

        public virtual Countries FkCountryNavigation { get; set; }
        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<AppUserProfiles> AppUserProfiles { get; set; }
        public virtual ICollection<Citydetails> Citydetails { get; set; }
        public virtual ICollection<UserBusinessProfile> UserBusinessProfile { get; set; }
    }
}
