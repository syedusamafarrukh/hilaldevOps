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
using Hilal.DataViewModel.Request;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class DashboardSliderService : IDashboardSliderService
    {
        public GetDashboardSliderResponse GetDashboardSlider(long LanguageId)
        {
            try
            {
                GetDashboardSliderResponse response = new GetDashboardSliderResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.DashboardSlider
                        .Include(x => x.DashboardSliderDetails)
                        .OrderByDescending(x => x.CreatedOn)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetGetDashboardSliderResponse
                        {
                            Id = x.Id,
                            Description = x.DashboardSliderDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Description,
                            Title = x.DashboardSliderDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Title,
                            Url = x.Url,
                            ThubnilUrl = x.ThumbnilUrl,
                        })
                        .AsQueryable();

                    response.ItemList = query.ToList();
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveDashboardSlider(CreateDashboardSliderRequest DashboardSliderRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<DashboardSliderDetails> informations = new List<DashboardSliderDetails>();

                foreach (var information in DashboardSliderRequest.SliderDetails)
                {
                    informations.Add(new DashboardSliderDetails
                    {
                        Id = SystemGlobal.GetId(),
                        Title = information.Title,
                        FkLanguageId = information.FK_LanguageId,
                        Description = information.Description,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                }


                if (DashboardSliderRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.DashboardSlider.AddAsync(new DashboardSlider
                                {
                                    Id = SystemGlobal.GetId(),
                                    Url = DashboardSliderRequest.Url,
                                    ThumbnilUrl = DashboardSliderRequest.ThubnilUrl,
                                    DashboardSliderDetails = informations,
                                    CreatedBy = userId.ToString(),
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedOn = DateTime.UtcNow,
                                    IsActive = true,
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
                                await db.DashboardSliderDetails
                                    .Where(x => x.IsActive == true && x.FkDashboardSliderId.Equals(DashboardSliderRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.DashboardSliderDetails
                                    .Where(x => x.IsActive == true && x.FkDashboardSliderId.Equals(DashboardSliderRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.DashboardSlider.FirstOrDefault(x => x.Id.Equals(DashboardSliderRequest.Id));

                                category.Url = DashboardSliderRequest.Url;
                                category.ThumbnilUrl = DashboardSliderRequest.ThubnilUrl;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkDashboardSliderId = category.Id);
                                await db.DashboardSliderDetails.AddRangeAsync(informations);

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

        public CreateDashboardSliderRequest GetEditDashboardSlider(Guid id)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.DashboardSlider
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateDashboardSliderRequest
                        {
                            Id = x.Id,
                            SliderDetails = x.DashboardSliderDetails.Where(z => z.IsActive == true).Select(z => new CreateDashboardSliderDetailsRequest
                            {
                                FK_LanguageId = z.FkLanguageId,
                                Title = z.Title,
                                Description = z.Description,
                                FK_DashboardSliderId = z.FkDashboardSliderId
                            }).ToList(),
                            Url = x.Url,
                            ThubnilUrl = x.ThumbnilUrl
                        }).FirstOrDefault() ?? new CreateDashboardSliderRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllDashboardSliderActivation(Guid DashboardSliderId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.DashboardSlider.FirstOrDefault(x => x.Id.Equals(DashboardSliderId));

                    if (category == null) throw new Exception("DashboardSliderId Doesn't Exists");

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

        public List<GetGetDashboardSliderResponseList> GetDashboardSliderList(int LanguageId)
        {
            try
            {
                List<GetGetDashboardSliderResponseList> response = new List<GetGetDashboardSliderResponseList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.DashboardSlider
                        .Include(x => x.DashboardSliderDetails)
                        .OrderByDescending(x => x.CreatedDate)
                        .Where(x => x.IsActive == true)
                        .Select(x => new GetGetDashboardSliderResponseList
                        {
                            Id = x.Id,
                            Description = x.DashboardSliderDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Description,
                            Title = x.DashboardSliderDetails.FirstOrDefault(y => y.FkLanguageId == LanguageId).Title,
                            Url = x.Url,
                            ThubnilUrl = x.ThumbnilUrl
                        }).AsQueryable();

                    response = query.ToList();
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
