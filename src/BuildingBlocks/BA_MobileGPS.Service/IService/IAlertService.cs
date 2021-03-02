using BA_MobileGPS.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAlertService
    {
        Task<AlertOnlineViewModel> GetListAlertOnlineAsync(AlertGetRequest request);

        Task<List<AlertTypeModel>> GetAlertTypeAsync(int CompanyID);

        Task<bool> HandleAlertAsync(StatusAlertRequestModel rqModel);

        Task<int> GetCountAlert(Guid PK_UserID);

        Task<List<AlertCompanyConfigRespone>> GetAlertCompanyConfig(int companyID);

        Task<AlertUserConfigurationsRespone> GetAlertUserConfigurations(Guid userId);

        Task<ResponseBaseV2<bool>> SendAlertUserConfig(AlertUserConfigurationsRequest request);
    }
}