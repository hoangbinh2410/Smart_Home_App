using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities.Constant;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class AppDeviceService : IAppDeviceService
    {
        private readonly IRequestProvider requestProvider;

        public AppDeviceService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<BaseResponse<bool>> InsertOrUpdateAppDevice(AppDeviceRequest request)
        {
            return await requestProvider.PostAsync<AppDeviceRequest, BaseResponse<bool>>(ApiUri.INSERT_UPDATE_APP_DEVICE, request);
        }
    }
}