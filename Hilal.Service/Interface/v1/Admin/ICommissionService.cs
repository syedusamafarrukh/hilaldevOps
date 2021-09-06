using Hilal.DataViewModel.Request.Admin.v1;
using Hilal.DataViewModel.Response.Admin.v1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hilal.Service.Interface.v1.Admin
{
    public interface ICommissionService
    {
        GetCommissionResponse GetCommission(ListGeneralModel Commission);
        Task<bool> SaveCommission(CreateCommissionRequest CommissionRequest, Guid userId);
        CreateCommissionRequest GetEditCommission(Guid id, Guid userId);
        Task<bool> ControllCommissionActivation(Guid CommissionId, bool activation, Guid userId);
    }
}
