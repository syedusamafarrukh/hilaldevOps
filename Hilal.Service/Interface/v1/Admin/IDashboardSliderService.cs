using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface IDashboardSliderService
    {
        GetDashboardSliderResponse GetDashboardSlider(long LanguageId);
        Task<bool> SaveDashboardSlider(CreateDashboardSliderRequest DashboardSliderRequest, Guid userId);
        CreateDashboardSliderRequest GetEditDashboardSlider(Guid id);
        Task<bool> ControllDashboardSliderActivation(Guid DashboardSliderId, bool activation, Guid userId);
        List<GetGetDashboardSliderResponseList> GetDashboardSliderList(int LanguageId);
    }
}
