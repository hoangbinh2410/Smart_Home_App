using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IServicePackageHistoryService : IReportBaseService<ServicePackHistoryRequest, List<ServicePackHistory>>
    {
        Task<BaseResponse<ServicePackageInfo>> GetCurrentServicePack(object request);

        Task<BaseResponse<List<ShipPackage>>> GetShipPackages();
    }
}