using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service
{
   public class GetStatusService : IGetStatusService
    {
        private readonly IRequestProvider _requestProvider;
        public GetStatusService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<StastusSmartHomeResponse> Getall()
        {
            StastusSmartHomeResponse result = new StastusSmartHomeResponse();
            try
            {              
                var response = await _requestProvider.GetAsync<StastusSmartHomeResponse>("http://192.168.0.104:8000/api/v1");
                if (response != null )
                {
                    result = response;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}
