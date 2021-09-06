using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.Admin;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Enum;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class ApprovelSettingsService : IApprovelSettingsService
    {
        public List<CreateApprovelSettingRequest> GetApprovelSetting(int? Id)
        {
            try
            {
                List<CreateApprovelSettingRequest> response = new List<CreateApprovelSettingRequest>();

                using (var db = new HilalDbContext())
                {
                    response = db.ApprovelSettings
                        .Where(x => x.IsActive == true && (Id == null ? true : x.CategoryType.Equals(Id)))
                        .Select(x => new CreateApprovelSettingRequest
                        {
                            Id = x.Id,
                            CategoryType = x.CategoryType,
                            ApprovelType = x.ApprovelType,
                        }).ToList();
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveApprovelSetting(CreateApprovelSettingRequest ApprovelSettingRequest, Guid userId)
        {
            try
            {
                bool response = false;

                if (ApprovelSettingRequest.Id == 0)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.ApprovelSettings.AddAsync(new ApprovelSettings
                                {
                                    Id = 0,
                                    CategoryType = ApprovelSettingRequest.CategoryType,
                                    ApprovelType = ApprovelSettingRequest.ApprovelType,
                                    IsActive = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedDate = DateTime.UtcNow
                                });

                                await db.SaveChangesAsync();
                                trans.Commit();

                                response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var category = db.ApprovelSettings.FirstOrDefault(x => x.Id.Equals(ApprovelSettingRequest.Id));

                                category.ApprovelType = ApprovelSettingRequest.ApprovelType;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                trans.Commit();

                                response = true;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CreateApprovelSettingRequest GetEditApprovelSetting(int id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.ApprovelSettings
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateApprovelSettingRequest
                        {
                            Id = x.Id,
                            CategoryType = x.CategoryType,
                            ApprovelType = x.ApprovelType
                        })
                        .FirstOrDefault() ?? new CreateApprovelSettingRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllApprovelSettingActivation(int ApprovelSettingId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.ApprovelSettings.FirstOrDefault(x => x.Id.Equals(ApprovelSettingId));

                    if (category == null) throw new Exception("ApprovelSettingsId Doesn't Exists");

                    category.IsActive = activation;
                    category.UpdatedBy = userId.ToString();
                    category.UpdatedDate = DateTime.UtcNow;

                    db.Entry(category).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
