using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Response.App.v1;

namespace Hilal.Data.Context
{
    public partial class HilalDbContext : DbContext
    {
        public HilalDbContext()
        {
        }

        public HilalDbContext(DbContextOptions<HilalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminUserRoles> AdminUserRoles { get; set; }
        public virtual DbSet<AdminUsers> AdminUsers { get; set; }
        public virtual DbSet<Advertisement> Advertisement { get; set; }
        public virtual DbSet<AdvertisementCommission> AdvertisementCommission { get; set; }
        public virtual DbSet<AdvertisementDetails> AdvertisementDetails { get; set; }
        public virtual DbSet<AdvertisementNotifications> AdvertisementNotifications { get; set; }
        public virtual DbSet<AdvertisementStatus> AdvertisementStatus { get; set; }
        public virtual DbSet<Age> Age { get; set; }
        public virtual DbSet<AgeCategories> AgeCategories { get; set; }
        public virtual DbSet<AgeDetails> AgeDetails { get; set; }
        public virtual DbSet<AppUserProfiles> AppUserProfiles { get; set; }
        public virtual DbSet<AppUserSubscription> AppUserSubscription { get; set; }
        public virtual DbSet<AppUsers> AppUsers { get; set; }
        public virtual DbSet<ApprovelSettings> ApprovelSettings { get; set; }
        public virtual DbSet<Attachement> Attachement { get; set; }
        public virtual DbSet<Blogs> Blogs { get; set; }
        public virtual DbSet<Breed> Breed { get; set; }
        public virtual DbSet<BreedCategories> BreedCategories { get; set; }
        public virtual DbSet<BreedDetails> BreedDetails { get; set; }
        public virtual DbSet<BuinessProfileDetails> BuinessProfileDetails { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CategoriesDetails> CategoriesDetails { get; set; }
        public virtual DbSet<ChatMessages> ChatMessages { get; set; }
        public virtual DbSet<ChatNotifications> ChatNotifications { get; set; }
        public virtual DbSet<ChatThreads> ChatThreads { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Citydetails> Citydetails { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Commission> Commission { get; set; }
        public virtual DbSet<CommissionDetails> CommissionDetails { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<CountryDetails> CountryDetails { get; set; }
        public virtual DbSet<DashboardSlider> DashboardSlider { get; set; }
        public virtual DbSet<DashboardSliderDetails> DashboardSliderDetails { get; set; }
        public virtual DbSet<DeviceTypes> DeviceTypes { get; set; }
        public virtual DbSet<FeaturedAdvertisements> FeaturedAdvertisements { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<GenderDetails> GenderDetails { get; set; }
        public virtual DbSet<GuestAppUserDeviceInformations> GuestAppUserDeviceInformations { get; set; }
        public virtual DbSet<HilalGenders> HilalGenders { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<PaymentHistory> PaymentHistory { get; set; }
        public virtual DbSet<Rights> Rights { get; set; }
        public virtual DbSet<RoleRights> RoleRights { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Subsciption> Subsciption { get; set; }
        public virtual DbSet<SubscriptionDetails> SubscriptionDetails { get; set; }
        public virtual DbSet<UserBookmarks> UserBookmarks { get; set; }
        public virtual DbSet<UserBusinessProfile> UserBusinessProfile { get; set; }
        public virtual DbSet<UserCards> UserCards { get; set; }
        public virtual DbSet<UserDeviceInformations> UserDeviceInformations { get; set; }
        public virtual DbSet<WhatsAppInfo> WhatsAppInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Initial Catalog= sd_HilalMarket_Dev;user id=Hilal_Prod;password=Hilal654;Data Source=10.0.0.10;MultipleActiveResultSets=True", x => x.UseNetTopologySuite());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
            modelBuilder.Entity<GetAdvertisementRes>().HasKey(e => new { e.Id, });
            modelBuilder.Entity<GetBusinessProfileSP>().HasKey(e => new { e.Id, });
            modelBuilder.Entity<AdminUserRoles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.HasOne(d => d.AdminUser)
                    .WithMany(p => p.AdminUserRoles)
                    .HasForeignKey(d => d.AdminUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminUserRoles_AdminUsers");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AdminUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminUserRoles_Roles");
            });

            modelBuilder.Entity<AdminUsers>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.Designation)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ImageThumbnailUrl).HasMaxLength(500);

                entity.Property(e => e.ImageUrl).HasMaxLength(500);

                entity.Property(e => e.IsSuperAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneCountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AdminUsers)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdminUsers_Gender");
            });

            modelBuilder.Entity<Advertisement>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CommissionAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkAgeId).HasColumnName("FK_AgeId");

                entity.Property(e => e.FkAppUserId).HasColumnName("FK_AppUserId");

                entity.Property(e => e.FkBreedId).HasColumnName("FK_BreedId");

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkCityId).HasColumnName("FK_CityId");

                entity.Property(e => e.FkGenderId).HasColumnName("FK_GenderId");

                entity.Property(e => e.FkStatusId).HasColumnName("FK_StatusId");

                entity.Property(e => e.FkSubCategoryId).HasColumnName("FK_SubCategoryId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MinimumPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PhoneNumberCountryCode).HasMaxLength(450);

                entity.Property(e => e.RefId).HasMaxLength(450);

                entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.WhatsAppCountryCode).HasMaxLength(450);

                entity.HasOne(d => d.FkAge)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.FkAgeId)
                    .HasConstraintName("FK_Advertisement_Age");

                entity.HasOne(d => d.FkAppUser)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.FkAppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Advertisement_AppUsers");

                entity.HasOne(d => d.FkBreed)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.FkBreedId)
                    .HasConstraintName("FK_Advertisement_Breed");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.AdvertisementFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Advertisement_Categories");

                entity.HasOne(d => d.FkCity)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.FkCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Advertisement_Cities");

                entity.HasOne(d => d.FkGender)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.FkGenderId)
                    .HasConstraintName("FK_Advertisement_HilalGenders");

                entity.HasOne(d => d.FkStatus)
                    .WithMany(p => p.Advertisement)
                    .HasForeignKey(d => d.FkStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Advertisement_AdvertisementStatus");

                entity.HasOne(d => d.FkSubCategory)
                    .WithMany(p => p.AdvertisementFkSubCategory)
                    .HasForeignKey(d => d.FkSubCategoryId)
                    .HasConstraintName("FK_Advertisement_Categories1");
            });

            modelBuilder.Entity<AdvertisementCommission>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.AdvertisementCommission)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdvertisementCommission_Advertisement");
            });

            modelBuilder.Entity<AdvertisementDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.Father).HasMaxLength(500);

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Mother).HasMaxLength(500);

                entity.Property(e => e.SellerName).HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.AdvertisementDetails)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdvertisementDetails_Advertisement");
            });

            modelBuilder.Entity<AdvertisementNotifications>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BodyText).IsRequired();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.FkServiceId).HasColumnName("FK_ServiceId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.AdminReceiver)
                    .WithMany(p => p.AdvertisementNotifications)
                    .HasForeignKey(d => d.AdminReceiverId)
                    .HasConstraintName("FK_AdvertisementNotifications_AdminUsers");

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.AdvertisementNotifications)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .HasConstraintName("FK_AdvertisementNotifications_Advertisement");

                entity.HasOne(d => d.FkService)
                    .WithMany(p => p.AdvertisementNotifications)
                    .HasForeignKey(d => d.FkServiceId)
                    .HasConstraintName("FK_AdvertisementNotifications_UserBusinessProfile");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.AdvertisementNotifications)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK_AdvertisementNotifications_AppUsers");
            });

            modelBuilder.Entity<AdvertisementStatus>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Age>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkSubCategoryId).HasColumnName("FK_SubCategoryId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.AgeFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .HasConstraintName("FK_Age_Categories");

                entity.HasOne(d => d.FkSubCategory)
                    .WithMany(p => p.AgeFkSubCategory)
                    .HasForeignKey(d => d.FkSubCategoryId)
                    .HasConstraintName("FK_Age_Categories1");
            });

            modelBuilder.Entity<AgeCategories>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FkAgeId).HasColumnName("FK_AgeId");

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkSubCategoryId).HasColumnName("FK_SubCategoryId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAge)
                    .WithMany(p => p.AgeCategories)
                    .HasForeignKey(d => d.FkAgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AgeCategories_Age");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.AgeCategoriesFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AgeCategories_Categories");

                entity.HasOne(d => d.FkSubCategory)
                    .WithMany(p => p.AgeCategoriesFkSubCategory)
                    .HasForeignKey(d => d.FkSubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AgeCategories_Categories1");
            });

            modelBuilder.Entity<AgeDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkAgeId).HasColumnName("FK_AgeId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAge)
                    .WithMany(p => p.AgeDetails)
                    .HasForeignKey(d => d.FkAgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AgeDetails_Age");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.AgeDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AgeDetails_Languages");
            });

            modelBuilder.Entity<AppUserProfiles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.ImageThumbnailUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nationality).HasMaxLength(100);

                entity.Property(e => e.PhoneCountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p.AppUserProfiles)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserProfiles_AppUsers");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.AppUserProfiles)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_AppUserProfiles_Cities");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.AppUserProfiles)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_AppUserProfiles_Gender");
            });

            modelBuilder.Entity<AppUserSubscription>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkSubscribedPlanId).HasColumnName("FK_SubscribedPlanId");

                entity.Property(e => e.FkUserId).HasColumnName("FK_UserId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkSubscribedPlan)
                    .WithMany(p => p.AppUserSubscription)
                    .HasForeignKey(d => d.FkSubscribedPlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserSubscription_Subsciption");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.AppUserSubscription)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AppUserSubscription_AppUsers");
            });

            modelBuilder.Entity<AppUsers>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Otp)
                    .IsRequired()
                    .HasColumnName("OTP")
                    .HasMaxLength(4);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);
            });

            modelBuilder.Entity<ApprovelSettings>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Attachement>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.FkBusinessProfileId).HasColumnName("FK_BusinessProfileId");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.Attachement)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .HasConstraintName("FK_Attachement_Advertisement");

                entity.HasOne(d => d.FkBusinessProfile)
                    .WithMany(p => p.Attachement)
                    .HasForeignKey(d => d.FkBusinessProfileId)
                    .HasConstraintName("FK_Attachement_UserBusinessProfile");
            });

            modelBuilder.Entity<Blogs>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.HeaderImage).IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsPublished)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ThunbnilUrl).IsRequired();

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.UpdateBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Breed>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkSelfBreedId).HasColumnName("FK_SelfBreedId");

                entity.Property(e => e.FkSubCategoryId).HasColumnName("FK_SubCategoryId");

                entity.Property(e => e.ImageThumbnailUrl).IsRequired();

                entity.Property(e => e.ImageUrl).IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.BreedFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .HasConstraintName("FK_Breed_Categories");

                entity.HasOne(d => d.FkSelfBreed)
                    .WithMany(p => p.InverseFkSelfBreed)
                    .HasForeignKey(d => d.FkSelfBreedId)
                    .HasConstraintName("FK_Breed_Breed");

                entity.HasOne(d => d.FkSubCategory)
                    .WithMany(p => p.BreedFkSubCategory)
                    .HasForeignKey(d => d.FkSubCategoryId)
                    .HasConstraintName("FK_Breed_Categories1");
            });

            modelBuilder.Entity<BreedCategories>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FkBreedId).HasColumnName("FK_BreedId");

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkSubCategoryId).HasColumnName("FK_SubCategoryId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkBreed)
                    .WithMany(p => p.BreedCategories)
                    .HasForeignKey(d => d.FkBreedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BreedCategories_Breed");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.BreedCategoriesFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BreedCategories_Categories");

                entity.HasOne(d => d.FkSubCategory)
                    .WithMany(p => p.BreedCategoriesFkSubCategory)
                    .HasForeignKey(d => d.FkSubCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BreedCategories_Categories1");
            });

            modelBuilder.Entity<BreedDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkBreedId).HasColumnName("FK_BreedId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkBreed)
                    .WithMany(p => p.BreedDetails)
                    .HasForeignKey(d => d.FkBreedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BreedDetails_Breed");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.BreedDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BreedDetails_Languages");
            });

            modelBuilder.Entity<BuinessProfileDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.FkUserBusinessProfileId).HasColumnName("FK_UserBusinessProfileId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SellerName).HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.BuinessProfileDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuinessProfileDetails_Languages");

                entity.HasOne(d => d.FkUserBusinessProfile)
                    .WithMany(p => p.BuinessProfileDetails)
                    .HasForeignKey(d => d.FkUserBusinessProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BuinessProfileDetails_UserBusinessProfile");
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCategoryType).HasColumnName("FK_CategoryType");

                entity.Property(e => e.ImageThumbnailUrl).IsRequired();

                entity.Property(e => e.ImageUrl).IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsSubCategory)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.CategorySelf)
                    .WithMany(p => p.InverseCategorySelf)
                    .HasForeignKey(d => d.CategorySelfId)
                    .HasConstraintName("FK_Categories_Categories");
            });

            modelBuilder.Entity<CategoriesDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.CategoriesDetails)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoriesDetails_Categories");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.CategoriesDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoriesDetails_Languages");
            });

            modelBuilder.Entity<ChatMessages>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkChatThreadsId).HasColumnName("FK_ChatThreadsId");

                entity.Property(e => e.FkSenderId).HasColumnName("FK_SenderId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MessageText).IsRequired();

                entity.HasOne(d => d.FkChatThreads)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.FkChatThreadsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessages_ChatThreads");

                entity.HasOne(d => d.FkSender)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.FkSenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatMessages_AppUsers");
            });

            modelBuilder.Entity<ChatNotifications>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BodyText).IsRequired();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FkChatThreadId).HasColumnName("FK_ChatThreadId");

                entity.Property(e => e.FkReceiverId).HasColumnName("FK_ReceiverId");

                entity.Property(e => e.FkSenderId).HasColumnName("FK_SenderId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FkChatThread)
                    .WithMany(p => p.ChatNotifications)
                    .HasForeignKey(d => d.FkChatThreadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatNotifications_ChatThreads");

                entity.HasOne(d => d.FkReceiver)
                    .WithMany(p => p.ChatNotificationsFkReceiver)
                    .HasForeignKey(d => d.FkReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatNotifications_AppUsers");

                entity.HasOne(d => d.FkSender)
                    .WithMany(p => p.ChatNotificationsFkSender)
                    .HasForeignKey(d => d.FkSenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatNotifications_AppUsers1");
            });

            modelBuilder.Entity<ChatThreads>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Deleted1By).HasMaxLength(450);

                entity.Property(e => e.Deleted1Date).HasColumnType("datetime");

                entity.Property(e => e.Deleted2By).HasMaxLength(450);

                entity.Property(e => e.Deleted2Date).HasColumnType("datetime");

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.FkSellerId).HasColumnName("FK_SellerId");

                entity.Property(e => e.FkServiceId).HasColumnName("FK_ServiceId");

                entity.Property(e => e.FkUserId).HasColumnName("FK_UserId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.ChatThreads)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .HasConstraintName("FK_ChatThreads_Advertisement");

                entity.HasOne(d => d.FkSeller)
                    .WithMany(p => p.ChatThreadsFkSeller)
                    .HasForeignKey(d => d.FkSellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatThreads_AppUsers");

                entity.HasOne(d => d.FkService)
                    .WithMany(p => p.ChatThreads)
                    .HasForeignKey(d => d.FkServiceId)
                    .HasConstraintName("FK_ChatThreads_UserBusinessProfile");

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.ChatThreadsFkUser)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatThreads_AppUsers1");
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCountry).HasColumnName("FK_Country");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCountryNavigation)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.FkCountry)
                    .HasConstraintName("FK_Cities_Countries");
            });

            modelBuilder.Entity<Citydetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCityId).HasColumnName("FK_CityId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCity)
                    .WithMany(p => p.Citydetails)
                    .HasForeignKey(d => d.FkCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Citydetails_Cities");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.Citydetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Citydetails_Languages");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Comments1)
                    .IsRequired()
                    .HasColumnName("Comments");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.FkServiceId).HasColumnName("FK_ServiceId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .HasConstraintName("FK_Comments_Advertisement");

                entity.HasOne(d => d.FkService)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FkServiceId)
                    .HasConstraintName("FK_Comments_UserBusinessProfile");
            });

            modelBuilder.Entity<Commission>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.Commission)
                    .HasForeignKey(d => d.FkCategoryId)
                    .HasConstraintName("FK_Commission_Categories");
            });

            modelBuilder.Entity<CommissionDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DisplayPercentage).HasMaxLength(500);

                entity.Property(e => e.DisplayRange)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FkCommissionId).HasColumnName("FK_CommissionId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCommission)
                    .WithMany(p => p.CommissionDetails)
                    .HasForeignKey(d => d.FkCommissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommissionDetails_Commission");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.CommissionDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommissionDetails_Languages");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CountryDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkCountryId).HasColumnName("FK_CountryId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkCountry)
                    .WithMany(p => p.CountryDetails)
                    .HasForeignKey(d => d.FkCountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_1_Countries");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.CountryDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_1_Languages");
            });

            modelBuilder.Entity<DashboardSlider>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.ThumbnilUrl).IsRequired();

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Url).IsRequired();
            });

            modelBuilder.Entity<DashboardSliderDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkDashboardSliderId).HasColumnName("FK_DashboardSliderId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkDashboardSlider)
                    .WithMany(p => p.DashboardSliderDetails)
                    .HasForeignKey(d => d.FkDashboardSliderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DashboardSliderDetails_DashboardSlider");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.DashboardSliderDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DashboardSliderDetails_Languages");
            });

            modelBuilder.Entity<DeviceTypes>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsEnabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<FeaturedAdvertisements>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.FeaturedAdvertisements)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FeaturedAdvertisements_Advertisement");
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenderDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.FkGenderId).HasColumnName("FK_GenderId");

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkGender)
                    .WithMany(p => p.GenderDetails)
                    .HasForeignKey(d => d.FkGenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenderDetails_Genders");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.GenderDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenderDetails_Languages");
            });

            modelBuilder.Entity<GuestAppUserDeviceInformations>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.DeviceToken).HasMaxLength(500);

                entity.Property(e => e.IsEnabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.Version).HasMaxLength(500);

                entity.Property(e => e.VersionName).HasMaxLength(500);

                entity.HasOne(d => d.DeviceType)
                    .WithMany(p => p.GuestAppUserDeviceInformations)
                    .HasForeignKey(d => d.DeviceTypeId)
                    .HasConstraintName("FK_GuestAppUserDeviceInformations_DeviceTypes");
            });

            modelBuilder.Entity<HilalGenders>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(50);

                entity.Property(e => e.DeletedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p.PaymentHistory)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentHistory_AppUsers");
            });

            modelBuilder.Entity<Rights>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);
            });

            modelBuilder.Entity<RoleRights>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.HasOne(d => d.Right)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RightId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleRights_Rights");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleRights)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleRights_Roles");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.IsSuperAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);
            });

            modelBuilder.Entity<Subsciption>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsBlock).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDisplayed).HasDefaultValueSql("((1))");

                entity.Property(e => e.NewAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SubscriptionDetails>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DisplayValidityDays).HasMaxLength(500);

                entity.Property(e => e.FkLanguageId).HasColumnName("FK_LanguageId");

                entity.Property(e => e.FkSubscriptionId).HasColumnName("FK_SubscriptionId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkLanguage)
                    .WithMany(p => p.SubscriptionDetails)
                    .HasForeignKey(d => d.FkLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubscriptionDetails_Languages");

                entity.HasOne(d => d.FkSubscription)
                    .WithMany(p => p.SubscriptionDetails)
                    .HasForeignKey(d => d.FkSubscriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubscriptionDetails_Subsciption");
            });

            modelBuilder.Entity<UserBookmarks>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FkAdvertisementId).HasColumnName("FK_AdvertisementId");

                entity.Property(e => e.FkAppUserId).HasColumnName("FK_AppUserId");

                entity.Property(e => e.FkServiceId).HasColumnName("FK_ServiceId");

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.HasOne(d => d.FkAdvertisement)
                    .WithMany(p => p.UserBookmarks)
                    .HasForeignKey(d => d.FkAdvertisementId)
                    .HasConstraintName("FK_UserBookmarks_Advertisement");

                entity.HasOne(d => d.FkAppUser)
                    .WithMany(p => p.UserBookmarks)
                    .HasForeignKey(d => d.FkAppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBookmarks_AppUsers");

                entity.HasOne(d => d.FkService)
                    .WithMany(p => p.UserBookmarks)
                    .HasForeignKey(d => d.FkServiceId)
                    .HasConstraintName("FK_UserBookmarks_UserBusinessProfile");
            });

            modelBuilder.Entity<UserBusinessProfile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContactNumber).HasMaxLength(500);

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.EmailId).HasMaxLength(500);

                entity.Property(e => e.FkAppUserId).HasColumnName("FK_AppUserId");

                entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryId");

                entity.Property(e => e.FkCityId).HasColumnName("FK_CityId");

                entity.Property(e => e.FkStatusId).HasColumnName("FK_StatusId");

                entity.Property(e => e.FkSubCategoryId).HasColumnName("FK_SubCategoryId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.PhoneNumberCountryCode).HasMaxLength(450);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.WhatsAppCountryCode).HasMaxLength(450);

                entity.Property(e => e.WhatsAppNumber).HasMaxLength(500);

                entity.HasOne(d => d.FkAppUser)
                    .WithMany(p => p.UserBusinessProfile)
                    .HasForeignKey(d => d.FkAppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBusinessProfile_AppUsers");

                entity.HasOne(d => d.FkCategory)
                    .WithMany(p => p.UserBusinessProfileFkCategory)
                    .HasForeignKey(d => d.FkCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBusinessProfile_Categories1");

                entity.HasOne(d => d.FkCity)
                    .WithMany(p => p.UserBusinessProfile)
                    .HasForeignKey(d => d.FkCityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBusinessProfile_Cities");

                entity.HasOne(d => d.FkStatus)
                    .WithMany(p => p.UserBusinessProfile)
                    .HasForeignKey(d => d.FkStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserBusinessProfile_AdvertisementStatus");

                entity.HasOne(d => d.FkSubCategory)
                    .WithMany(p => p.UserBusinessProfileFkSubCategory)
                    .HasForeignKey(d => d.FkSubCategoryId)
                    .HasConstraintName("FK_UserBusinessProfile_Categories");
            });

            modelBuilder.Entity<UserCards>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedBy).HasMaxLength(450);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Csv)
                    .IsRequired()
                    .HasColumnName("CSV")
                    .HasMaxLength(500);

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.FkAppUserId).HasColumnName("FK_AppUserId");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.FkAppUser)
                    .WithMany(p => p.UserCards)
                    .HasForeignKey(d => d.FkAppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCards_AdminUsers");
            });

            modelBuilder.Entity<UserDeviceInformations>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedOnDate).HasColumnType("date");

                entity.Property(e => e.DeletedBy).HasMaxLength(450);

                entity.Property(e => e.DeviceToken).HasMaxLength(2000);

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.UpdatedBy).HasMaxLength(450);

                entity.Property(e => e.Version).HasMaxLength(300);

                entity.Property(e => e.VersionName).HasMaxLength(500);

                entity.HasOne(d => d.AppUser)
                    .WithMany(p => p.UserDeviceInformations)
                    .HasForeignKey(d => d.AppUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDeviceInformations_AppUsers");
            });

            modelBuilder.Entity<WhatsAppInfo>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.WhatsappNumber)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.WhatsappUrl).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
