using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class PingServerService : IPingServerService
    {
        private readonly IRequestProvider requestProvider;

        public PingServerService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<BaseResponse<bool>> PingServerStatus()
        {
            BaseResponse<bool> result = new BaseResponse<bool>();
            try
            {
                var url = string.Format(ApiUri.GET_PING_SERVER_STATUS);

                var data = await requestProvider.GetAsync<BaseResponse<bool>>(url);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

    }
}