using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class AppUserProfiles
    {
        public Guid Id { get; set; }
        public Guid AppUserId { get; set; }
        public int? GenderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public Guid? CityId { get; set; }

        public virtual AppUsers AppUser { get; set; }
        public virtual Cities City { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
