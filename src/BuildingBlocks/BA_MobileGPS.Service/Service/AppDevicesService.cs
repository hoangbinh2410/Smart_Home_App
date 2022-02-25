using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Reflection;
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

        public async Task<bool> InsertOrUpdateAppDevice(AppDeviceRequest request)
        {
            bool result = false;
            try
            {
                var respone = await requestProvider.PostAsync<AppDeviceRequest, ResponseBase<bool>>(ApiUri.INSERT_UPDATE_APP_DEVICE, request);

                if (respone != null && respone.Data)
                {
                    result = respone.Data;
                }
            }
             catch(Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
            
        }
    }
}