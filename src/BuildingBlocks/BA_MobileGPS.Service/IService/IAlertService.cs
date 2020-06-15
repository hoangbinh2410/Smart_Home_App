using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAlertService
    {
        Task<AlertOnlineViewModel> GetListAlertOnlineAsync(AlertGetRequest request);

        Task<List<AlertTypeModel>> GetAlertTypeAsync(Guid userId, string cultureName = "en");

        Task<bool> HandleAlertAsync(StatusAlertRequestModel rqModel);

        Task<int> GetCountAlert(Guid PK_UserID);
    }
}