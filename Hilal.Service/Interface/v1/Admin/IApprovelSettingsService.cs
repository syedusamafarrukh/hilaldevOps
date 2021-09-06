using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IApprovelSettingsService
    {
        List<CreateApprovelSettingRequest> GetApprovelSetting(int? Id);
        Task<bool> SaveApprovelSetting(CreateApprovelSettingRequest ApprovelSettingRequest, Guid userId);
        CreateApprovelSettingRequest GetEditApprovelSetting(int id, Guid userId);
        Task<bool> ControllApprovelSettingActivation(int ApprovelSettingId, bool activation, Guid userId);
    }
}
