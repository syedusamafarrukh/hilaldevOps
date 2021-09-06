using Hilal.Common;
using Hilal.Data.Context;
using Hilal.Data.DTOs;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Request;
using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response;
using Hilal.DataViewModel.Response.Admin.v1;
using Hilal.Service.Interface.v1.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Implementation.v1.Admin
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration configuration;

        public AccountService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region Login
        public Tuple<AdminLoginResponse, bool, bool> AdminLogin(AdminLoginRequest request)
        {
            string encryptionKey = configuration.GetValue<string>("EncryptionKey"), tokenKey = configuration.GetValue<string>("Tokens:Key");

            try
            {
                var encryptedPassword = new Encryption().Encrypt(request.Password, encryptionKey);
                var response = new AdminLoginResponse();
                bool isLogin = false,
                    isBlock = false;

                using (var db = new HilalDbContext())
                {
                    var user = db.AdminUsers
                        .Include(x => x.AdminUserRoles).FirstOrDefault(x => x.Email.ToLower().Equals(request.UserInfo.ToLower()) && x.Password.Equals(encryptedPassword));

                    if (user == null) return Tuple.Create(response, isLogin, isBlock);
                    isBlock = user.IsBlocked;
                    if (isBlock) return Tuple.Create(response, isLogin, isBlock);

                    var authToken = new Encryption().GetToken(new AdminAuthToken { UserId = user.Id, RoleId = user.AdminUserRoles.FirstOrDefault(x => x.IsEnabled).RoleId }, user.Id, tokenKey);

                    var RoleId = user.AdminUserRoles.FirstOrDefault(x => x.IsEnabled).RoleId;

                    var rights = db.RoleRights.Include(x=> x.Right).Where(x => x.RoleId == RoleId && x.IsEnabled == true)
                        .Select(x=> new General<Guid>
                        {
                            Id = x.RightId,
                            Name = x.Right.Name,
                        }).ToList();

                    response = new AdminLoginResponse
                    {
                        AccessToken = authToken,
                        RightsId = rights,
                    };
                    isLogin = true;
                    return Tuple.Create(response, isLogin, isBlock);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Dashboard Management
        #region Roles Rights Management
        public async Task<bool> AddRole(string name, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    if (db.Roles.Any(x => x.Name.ToLower().Equals(name.ToLower()))) throw new Exception("Role Already Exists");

                    await db.Roles.AddAsync(new Roles
                    {
                        Id = SystemGlobal.GetId(),
                        Name = name,
                        IsEnabled = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedOnDate = DateTime.UtcNow
                    });
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

        public async Task<bool> UpdateRole(UpdateRoleRequest roleRequest, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var role = db.Roles.FirstOrDefault(x => x.Id.Equals(roleRequest.Id) && x.IsEnabled);

                    if (role == null) throw new Exception("Role Id Doesn't Exists");

                    role.Name = roleRequest.Name;
                    role.UpdatedBy = userId.ToString();
                    role.UpdatedOn = DateTime.UtcNow;

                    db.Entry(role).State = EntityState.Modified;
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

        public async Task<bool> ControllRoleActivation(Guid roleId, bool activation, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var role = db.Roles.FirstOrDefault(x => x.Id.Equals(roleId) && x.IsEnabled);

                    if (role == null) throw new Exception("Role Id Doesn't Exists");

                    role.IsEnabled = activation;
                    role.UpdatedBy = userId.ToString();
                    role.UpdatedOn = DateTime.UtcNow;

                    db.Entry(role).State = EntityState.Modified;
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

        public List<General<Guid>> GetRoles()
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Roles.Where(x => x.IsEnabled &&  x.IsSuperAdmin == false).Select(x => new General<Guid>
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<General<Guid>> GetRights()
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.Rights.Where(x => x.IsEnabled).Select(x => new General<Guid>
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<General<Guid>> GetRights(Guid roleId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.RoleRights.Where(x => x.RoleId.Equals(roleId) && x.IsEnabled).Select(x => new General<Guid>
                    {
                        Id = x.RightId,
                        Name = x.Right.Name
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AssignRights(AssignRight assignRight, Guid userId)
        {
            try
            {
                List<RoleRights> rights = new List<RoleRights>();
                bool response = false;

                foreach (var rightId in assignRight.RightIds)
                {
                    rights.Add(new RoleRights
                    {
                        Id = SystemGlobal.GetId(),
                        RoleId = assignRight.RoleId,
                        RightId = rightId,
                        IsEnabled = true,
                        CreatedBy = userId.ToString(),
                        CreatedOn = DateTime.UtcNow,
                        CreatedOnDate = DateTime.UtcNow
                    });
                }

                using (var db = new HilalDbContext())
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //remove Older One
                            await db.RoleRights.Where(x => x.RoleId.Equals(assignRight.RoleId) && x.IsEnabled).ForEachAsync(x => { x.IsEnabled = false; x.DeletedBy = userId.ToString(); x.DeletedOn = DateTime.UtcNow; });
                            await db.SaveChangesAsync();

                            //Add new
                            await db.RoleRights.AddRangeAsync(rights);
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

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region User Management 
        public GetUsersResponse GetUsers(ListGeneralModel page)
        {
            try
            {
                GetUsersResponse response = new GetUsersResponse();

                using (var db = new HilalDbContext())
                {
                    var query = db.AdminUsers.Where(x => x.IsEnabled == true &&  x.IsSuperAdmin == false)
                        .Select(x => new CreateUserRequest
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CountryCode = x.PhoneCountryCode,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth,
                            Designation = x.Designation,
                            Email = x.Email,
                            Password = "***********",
                            GenderId = x.GenderId,
                            GenderName = x.Gender.Name,
                            RoleId = x.AdminUserRoles.FirstOrDefault(y => y.IsEnabled).RoleId,
                            RoleName = x.AdminUserRoles.FirstOrDefault(y => y.IsEnabled).Role.Name,
                            ProfileImage = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            }
                        })
                      .AsQueryable();

                    if (!string.IsNullOrEmpty(page.Search))
                    {
                        var date = new DateTime();
                        var sdate = DateTime.TryParse(page.Search, out date);
                        int totalCases = -1;
                        var isNumber = Int32.TryParse(page.Search, out totalCases);

                        query = query.Where(
                        x => x.Name.ToLower().Contains(page.Search.ToLower())
                    );
                    }

                    var orderedQuery = query.OrderByDescending(x => x.Name);
                    switch (page.SortIndex)
                    {
                        case 0:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                            break;
                        case 1:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.CountryCode) : query.OrderBy(x => x.CountryCode);
                            break;
                        case 2:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.PhoneNumber) : query.OrderBy(x => x.PhoneNumber);
                            break;
                        case 3:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.DateOfBirth) : query.OrderBy(x => x.DateOfBirth);
                            break;
                        case 4:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Designation) : query.OrderBy(x => x.Designation);
                            break;
                        case 5:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email);
                            break;
                        case 6:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.GenderName) : query.OrderBy(x => x.GenderName);
                            break;
                        case 7:
                            orderedQuery = page.SortBy == "desc" ? query.OrderByDescending(x => x.RoleName) : query.OrderBy(x => x.RoleName);
                            break;
                    }


                    response.Page = page.Page;
                    response.PageSize = page.PageSize;
                    response.TotalRecords = orderedQuery.Count();
                    if (page.PageSize > 0)
                    {
                        response.Users = orderedQuery.Skip(page.Page).Take(page.PageSize).ToList();
                    }
                    else
                    {
                        response.Users = orderedQuery.ToList();

                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveUser(CreateUserRequest createUser, Guid userId)
        {
            try
            {
                bool response = false;

                List<AdminUserRoles> userRoles = new List<AdminUserRoles>();

                userRoles.Add(new AdminUserRoles
                {
                    Id = SystemGlobal.GetId(),
                    RoleId = createUser.RoleId,
                    IsEnabled = true,
                    CreatedBy = userId.ToString(),
                    CreatedOn = DateTime.UtcNow,
                    CreatedOnDate = DateTime.UtcNow
                });

                if (createUser.Id == null)
                {
                    var encryptedPassword = new Encryption().Encrypt(createUser.Password, configuration.GetValue<string>("EncryptionKey"));

                    using (var db = new HilalDbContext())
                    {
                        if (db.AdminUsers.Any(x => x.Email.ToLower().Equals(createUser.Email.ToLower()))) throw new Exception("Email Already Exists");

                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                await db.AdminUsers.AddAsync(new AdminUsers
                                {
                                    Id = SystemGlobal.GetId(),
                                    Name = createUser.Name,
                                    Email = createUser.Email,
                                    Password = encryptedPassword,
                                    GenderId = createUser.GenderId,
                                    Designation = createUser.Designation,
                                    DateOfBirth = createUser.DateOfBirth,
                                    PhoneCountryCode = createUser.CountryCode,
                                    PhoneNumber = createUser.PhoneNumber,
                                    ImageThumbnailUrl = createUser.ProfileImage?.ThumbnailUrl ?? "",
                                    ImageUrl = createUser.ProfileImage?.URL ?? "",
                                    AdminUserRoles = userRoles,
                                    IsBlocked = false,
                                    IsEnabled = true,
                                    CreatedBy = userId.ToString(),
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedOnDate = DateTime.UtcNow
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
                                var user = db.AdminUsers.Find(createUser.Id);

                                if (user == null) throw new Exception("User Not Found");

                                //delete Old One
                                await db.AdminUserRoles.Where(x => x.IsEnabled && x.AdminUserId.Equals(user.Id)).ForEachAsync(x => { x.IsEnabled = false; x.DeletedBy = userId.ToString(); x.DeletedOn = DateTime.UtcNow; });
                                await db.SaveChangesAsync();

                                user.Name = createUser.Name;
                                user.Email = createUser.Email;
                                user.GenderId = createUser.GenderId;
                                user.Designation = createUser.Designation;
                                user.DateOfBirth = createUser.DateOfBirth;
                                user.PhoneCountryCode = createUser.CountryCode;
                                user.PhoneNumber = createUser.PhoneNumber;
                                user.ImageThumbnailUrl = createUser.ProfileImage?.ThumbnailUrl ?? "";
                                user.ImageUrl = createUser.ProfileImage?.URL ?? "";
                                user.UpdatedBy = userId.ToString();
                                user.UpdatedOn = DateTime.UtcNow;

                                db.Entry(user).State = EntityState.Modified;
                                await db.SaveChangesAsync();

                                userRoles.ForEach(x => x.AdminUserId = user.Id);
                                await db.AdminUserRoles.AddRangeAsync(userRoles);
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

        public CreateUserRequest GetEditUser(Guid userId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    return db.AdminUsers
                        .Where(x => x.Id.Equals(userId))
                        .Select(x => new CreateUserRequest
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CountryCode = x.PhoneCountryCode,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth,
                            Designation = x.Designation,
                            Email = x.Email,
                            Password = "***********",
                            GenderId = x.GenderId,
                            GenderName = x.Gender.Name,
                            RoleId = x.AdminUserRoles.FirstOrDefault(y => y.IsEnabled).RoleId,
                            RoleName = x.AdminUserRoles.FirstOrDefault(y => y.IsEnabled).Role.Name,
                            ProfileImage = new FileUrlResponce
                            {
                                URL = x.ImageUrl,
                                ThumbnailUrl = x.ImageThumbnailUrl
                            }
                        })
                        .FirstOrDefault() ?? throw new Exception("User Not Found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ControllUserActivation(Guid adminUserId, bool activation, bool isRemove, Guid userId)
        {
            try
            {
                bool response = false;

                using (var db = new HilalDbContext())
                {
                    var user = db.AdminUsers.FirstOrDefault(x => x.Id.Equals(adminUserId) && x.IsEnabled);

                    if (user == null) throw new Exception("user Id Doesn't Exists");

                    if (isRemove)
                    {
                        user.IsEnabled = activation;
                    }
                    else
                    {
                        user.IsBlocked = activation;
                    }

                    user.UpdatedBy = userId.ToString();
                    user.UpdatedOn = DateTime.UtcNow;

                    db.Entry(user).State = EntityState.Modified;
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

        public async Task<bool> ChangePassword(AdminChangePasswordRequest changePassword, Guid userId)
        {
            try
            {
                string encryptionKey = configuration.GetValue<string>("EncryptionKey");
                bool response = false;

                var encryptedPassword = new Encryption().Encrypt(changePassword.OldPassword, encryptionKey);

                using (var db = new HilalDbContext())
                {
                    var user = db.AdminUsers.FirstOrDefault(x => x.Id.Equals(changePassword.UserId) && x.Password.Equals(encryptedPassword));

                    if (user == null) throw new Exception("user Doesn't Exists");

                    user.Password = new Encryption().Encrypt(changePassword.NewPassword, encryptionKey);
                    user.UpdatedOn = DateTime.UtcNow;
                    user.UpdatedBy = userId.ToString();

                    db.Entry(user).State = EntityState.Modified;
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
        #endregion
        #endregion

        #region Authorization
        public bool IsAdminUserAllowed(Guid userId, Guid roleId, Guid rightId)
        {
            try
            {
                using (var db = new HilalDbContext())
                {
                    if (!db.AdminUsers.Any(x => x.IsEnabled && x.Id.Equals(userId))) return false;

                    if (!db.RoleRights.Any(x => x.IsEnabled && x.RoleId.Equals(roleId) && x.RightId.Equals(rightId))) return false;

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
