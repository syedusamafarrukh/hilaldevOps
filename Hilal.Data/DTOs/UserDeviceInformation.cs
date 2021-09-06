using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class UserDeviceInformation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string VersionName { get; set; }
        public string Version { get; set; }
        public string DeviceToken { get; set; }
        public Guid FkUserId { get; set; }
        public int? DeviceTypeId { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
