using System;
using System.Collections.Generic;

namespace Hilal.Data.DTOs
{
    public partial class Languages
    {
        public Languages()
        {
            AgeDetails = new HashSet<AgeDetails>();
            BreedDetails = new HashSet<BreedDetails>();
            BuinessProfileDetails = new HashSet<BuinessProfileDetails>();
            CategoriesDetails = new HashSet<CategoriesDetails>();
            Citydetails = new HashSet<Citydetails>();
            CommissionDetails = new HashSet<CommissionDetails>();
            CountryDetails = new HashSet<CountryDetails>();
            DashboardSliderDetails = new HashSet<DashboardSliderDetails>();
            GenderDetails = new HashSet<GenderDetails>();
            SubscriptionDetails = new HashSet<SubscriptionDetails>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual ICollection<AgeDetails> AgeDetails { get; set; }
        public virtual ICollection<BreedDetails> BreedDetails { get; set; }
        public virtual ICollection<BuinessProfileDetails> BuinessProfileDetails { get; set; }
        public virtual ICollection<CategoriesDetails> CategoriesDetails { get; set; }
        public virtual ICollection<Citydetails> Citydetails { get; set; }
        public virtual ICollection<CommissionDetails> CommissionDetails { get; set; }
        public virtual ICollection<CountryDetails> CountryDetails { get; set; }
        public virtual ICollection<DashboardSliderDetails> DashboardSliderDetails { get; set; }
        public virtual ICollection<GenderDetails> GenderDetails { get; set; }
        public virtual ICollection<SubscriptionDetails> SubscriptionDetails { get; set; }
    }
}
