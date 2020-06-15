using BA_MobileGPS.Entities;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IAppDeviceService
    {
        Task<BaseResponse<bool>> InsertOrUpdateAppDevice(AppDeviceRequest request);
    }
}