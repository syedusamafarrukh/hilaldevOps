using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AdminUsers
    {
        public AdminUsers()
        {
            AdminUserRoles = new HashSet<AdminUserRoles>();
            AdvertisementNotifications = new HashSet<AdvertisementNotifications>();
            UserCards = new HashSet<UserCards>();
        }

        public Guid Id { get; set; }
        public int GenderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Designation { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public bool? IsSuperAdmin { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual ICollection<AdminUserRoles> AdminUserRoles { get; set; }
        public virtual ICollection<AdvertisementNotifications> AdvertisementNotifications { get; set; }
        public virtual ICollection<UserCards> UserCards { get; set; }
    }
}
