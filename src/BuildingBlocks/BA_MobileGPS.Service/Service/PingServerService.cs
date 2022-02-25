using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
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

        public async Task<ResponseBase<bool>> PingServerStatus()
        {
            ResponseBase<bool> result = new ResponseBase<bool>();
            //try
            //{
            //    var url = string.Format(ApiUri.GET_PING_SERVER_STATUS);

            //    var data = await requestProvider.GetAsync<BaseResponse<bool>>(url);

            //    if (data != null)
            //    {
            //        result = data;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            //}
            return result;
        }

        public async Task<ResponseBase<DateTime>> GetTimeServer()
        {
            ResponseBase<DateTime> result = new ResponseBase<DateTime>();
            try
            {
                var url = string.Format(ApiUri.GET_TIMESERVER);

                var data = await requestProvider.GetAsync<ResponseBase<DateTime>>(url);

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