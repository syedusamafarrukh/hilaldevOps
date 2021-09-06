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
    public class CitiesService : ICitiesService
    {
        public GetCitiesResponse GetCities(GetCityRequest pCities)
        {
            try
            {
                GetCitiesResponse response = new GetCitiesResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Cities
                        .Where(x => x.IsActive == true &&  (pCities.countryId == null ? true : x.FkCountry == pCities.countryId))
                        .Select(x => new GetCities
                        {
                            Id = x.Id,
                            FkCountry = x.FkCountry,
                            Name = x.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == pCities.LanguageId).Name,
                            //IsActive = x.IsActive,
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(pCities.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(pCities.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(pCities.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(pCities.Search.ToLower())
                    );
                    }

                    var orderedQuery = query.OrderByDescending(x => x.Name);
                    switch (pCities.SortIndex)
                    {
                        case 0:
                            orderedQuery = pCities.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                    }


                    response.Page = pCities.Page;
                    response.PageSize = pCities.PageSize;
                    response.TotalRecords = orderedQuery.Count();
                    if (pCities.PageSize > 0)
                    {
                        response.ItemList = orderedQuery.Skip(pCities.Page).Take(pCities.PageSize).ToList();
                    }
                    else
                    {
                        response.ItemList = orderedQuery.ToList();
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveCities(CreateCitiesRequest CitiesRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<Citydetails> informations = new List<Citydetails>();

                foreach (var information in CitiesRequest.CitiesInformations)
                {
                    informations.Add(new Citydetails
                    {
                        Id = SystemGlobal.GetId(),
                        Name = information.Name,
                        FkLanguageId = information.LanguageId,
                        IsActive = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    });
                }

                if (CitiesRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Cities.AddAsync(new Cities
                                {
                                    Id = SystemGlobal.GetId(),
                                    Citydetails = informations,
                                    FkCountry = CitiesRequest.FkCountry,
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
                                await db.Citydetails
                                    .Where(x => x.IsActive ==  true && x.FkCityId.Equals(CitiesRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.Cities.FirstOrDefault(x => x.Id.Equals(CitiesRequest.Id));
                                
                                category.FkCountry = CitiesRequest.FkCountry;
                                category.IsActive = true;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkCityId = category.Id);
                                await db.Citydetails.AddRangeAsync(informations);

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

        public CreateCitiesRequest GetEditCities(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Cities
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateCitiesRequest
                        {
                            Id = x.Id,
                            FkCountry = x.FkCountry,
                            CitiesInformations = x.Citydetails.Where(y => y.IsActive == true).Select(y => new GenericDetailRequest
                            {
                                Id = y.Id,
                                Name = y.Name,
                                LanguageId = y.FkLanguageId,
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateCitiesRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllCitiesActivation(Guid CitiesId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Cities.FirstOrDefault(x => x.Id.Equals(CitiesId));

                    if (category == null) throw new Exception("CitiesId Doesn't Exists");

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

        public List<GetCitiesList> GetCitiesList(Guid? countryId, int LanguageId)
        {
            try
            {
                List<GetCitiesList> response = new List<GetCitiesList>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Cities
                        .Where(x => x.IsActive == true && (countryId == null ? true : x.FkCountry == countryId))
                        .Select(x => new GetCitiesList
                        {
                            Id = x.Id,
                            Name = x.Citydetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
                        }).AsQueryable();

                    response = query.OrderBy(x => x.Name).ToList();
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
