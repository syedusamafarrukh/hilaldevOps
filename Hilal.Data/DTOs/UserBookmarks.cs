using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class UserBookmarks
    {
        public Guid Id { get; set; }
        public Guid FkAppUserId { get; set; }
        public Guid? FkAdvertisementId { get; set; }
        public Guid? FkServiceId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual Advertisement FkAdvertisement { get; set; }
        public virtual AppUsers FkAppUser { get; set; }
        public virtual UserBusinessProfile FkService { get; set; }
    }
}
