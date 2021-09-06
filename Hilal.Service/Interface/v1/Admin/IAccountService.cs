using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Request;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hilal.DataViewModel.Response;
using Hilal.DataViewModel.Common;
using Hilal.DataViewModel.Response.Admin.v1;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IAccountService
    {
        Tuple<AdminLoginResponse, bool, bool> AdminLogin(AdminLoginRequest request);
        Task<bool> AddRole(string name, Guid userId);
        Task<bool> UpdateRole(UpdateRoleRequest roleRequest, Guid userId);
        Task<bool> ControllRoleActivation(Guid roleId, bool activation, Guid userId);
        List<General<Guid>> GetRoles();
        List<General<Guid>> GetRights();
        List<General<Guid>> GetRights(Guid roleId);
        Task<bool> AssignRights(AssignRight assignRight, Guid userId);
        GetUsersResponse GetUsers(ListGeneralModel page);
        Task<bool> SaveUser(CreateUserRequest createUser, Guid userId);
        CreateUserRequest GetEditUser(Guid userId);
        Task<bool> ControllUserActivation(Guid adminUserId, bool activation, bool isRemove, Guid userId);
        Task<bool> ChangePassword(AdminChangePasswordRequest changePassword, Guid userId);
        bool IsAdminUserAllowed(Guid userId, Guid roleId, Guid rightId);
    }
}
