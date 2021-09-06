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
    public class CountriesService : ICountriesService
    {
        public GetCountriesResponse GetCountries(ListGeneralModel pCountries)
        {
            try
            {
                GetCountriesResponse response = new GetCountriesResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.Countries
                        .Where(x => x.IsActive == true)
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(x => new GetCountries
                        {
                            Id = x.Id,
                            Name = x.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == pCountries.LanguageId).Name,
                            //IsActive = x.IsActive,
                        })
                        .AsQueryable();

                    if (!string.IsNullOrEmpty(pCountries.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(pCountries.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(pCountries.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(pCountries.Search.ToLower())
                    );
                    }

                    var orderedQuery = query;
                    switch (pCountries.SortIndex)
                    {
                        case 0:
                            orderedQuery = pCountries.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                    }


                    response.Page = pCountries.Page;
                    response.PageSize = pCountries.PageSize;
                    response.TotalRecords = orderedQuery.Count();
                    if (response.PageSize> 0)
                    {
                        response.ItemList = orderedQuery.Skip(pCountries.Page).Take(pCountries.PageSize).ToList();
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

        public async Task<bool> SaveCountries(CreateCountriesRequest CountriesRequest, Guid userId)
        {
            try
            {
                bool response = false;

                List<CountryDetails> informations = new List<CountryDetails>();

                foreach (var information in CountriesRequest.CountriesInformations)
                {
                    informations.Add(new CountryDetails
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

                if (CountriesRequest.Id == null)
                {
                    using (var db = new HilalDbContext())
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.Countries.AddAsync(new Countries
                                {
                                    Id = SystemGlobal.GetId(),
                                    CountryDetails = informations,
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
                                await db.CountryDetails
                                    .Where(x => x.IsActive ==  true && x.FkCountryId.Equals(CountriesRequest.Id))
                                    .ForEachAsync(x =>
                                    {
                                        x.IsActive = false;
                                        x.UpdatedBy = userId.ToString();
                                        x.UpdatedDate = DateTime.UtcNow;
                                    });

                                await db.SaveChangesAsync();

                                //add Newer

                                var category = db.Countries.FirstOrDefault(x => x.Id.Equals(CountriesRequest.Id));
                                
                                category.IsActive = true;
                                category.UpdatedBy = userId.ToString();
                                category.UpdatedDate = DateTime.UtcNow;

                                db.Entry(category).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                informations.ForEach(x => x.FkCountryId = category.Id);
                                await db.CountryDetails.AddRangeAsync(informations);

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

        public CreateCountriesRequest GetEditCountries(Guid id, Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Countries
                        .Where(x => x.Id.Equals(id))
                        .Select(x => new CreateCountriesRequest
                        {
                            Id = x.Id,
                            CountriesInformations = x.CountryDetails.Where(y => y.IsActive == true).Select(y => new GenericDetailRequest
                            {
                                Id = y.Id,
                                Name = y.Name
                            }).ToList()
                        })
                        .FirstOrDefault() ?? new CreateCountriesRequest();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllCountriesActivation(Guid CountriesId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var category = db.Countries.FirstOrDefault(x => x.Id.Equals(CountriesId));

                    if (category == null) throw new Exception("CountriesId Doesn't Exists");

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

        public List<GetCountries> GetCountriesList(int LanguageId)
        {
            try
            {
                List<GetCountries> response = new List<GetCountries>();

                using (var db = new HilalDbContext())
                {
                    var query = db.Countries
                        .Where(x => x.IsActive == true)
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(x => new GetCountries
                        {
                            Id = x.Id,
                            Name = x.CountryDetails.FirstOrDefault(y => y.IsActive == true && y.FkLanguageId == LanguageId).Name,
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
