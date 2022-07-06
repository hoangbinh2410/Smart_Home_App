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

        public async Task<List<StastusSmartHomeResponse>> Getall()
        {
            List<StastusSmartHomeResponse> result = new List<StastusSmartHomeResponse>();
            try
            {              
                var response = await _requestProvider.GetAsync<ResponseBase<List<StastusSmartHomeResponse>>>(ApiUri.GET_ALL_STATUS);
                if (response != null && response.Data != null)
                {
                    result = response.Data;
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
