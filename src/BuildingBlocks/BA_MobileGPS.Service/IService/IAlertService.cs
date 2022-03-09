using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAlertService
    {
        Task<List<AlertOnlineDetailModel>> GetListAlertOnlineAsync(AlertGetRequest request);

        Task<List<AlertTypeModel>> GetAlertTypeAsync(int CompanyID);

        Task<bool> HandleAlertAsync(StatusAlertRequestModel rqModel);

        Task<int> GetCountAlert(GetCountAlertByUserIDRequest request);

        Task<List<AlertCompanyConfigRespone>> GetAlertCompanyConfig(int companyID);

        Task<AlertUserConfigurationsRespone> GetAlertUserConfigurations(Guid userId);

        Task<ResponseBase<bool>> SendAlertUserConfig(AlertUserConfigurationsRequest request);

        Task<AlertMaskModel> GetAlertMaskDetail(Guid id);
    }
}