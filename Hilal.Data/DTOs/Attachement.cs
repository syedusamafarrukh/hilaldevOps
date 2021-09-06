using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Attachement
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid? FkBusinessProfileId { get; set; }
        public Guid? FkAdvertisementId { get; set; }
        public bool IsVideo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public string ThumbnilUrl { get; set; }
        public string WaterMarkImage { get; set; }

        public virtual Advertisement FkAdvertisement { get; set; }
        public virtual UserBusinessProfile FkBusinessProfile { get; set; }
    }
}
