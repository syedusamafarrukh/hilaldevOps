using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class UserDeviceInformations
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string VersionName { get; set; }
        public string Version { get; set; }
        public string DeviceToken { get; set; }
        public Guid AppUserId { get; set; }
        public int? DeviceTypeId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public virtual AppUsers AppUser { get; set; }
    }
}
