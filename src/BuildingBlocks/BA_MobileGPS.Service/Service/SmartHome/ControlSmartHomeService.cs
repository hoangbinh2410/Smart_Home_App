using BA_MobileGPS.Entities;
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
    public class ControlSmartHomeService : IControlSmartHomeService
    {
        private readonly IRequestProvider _requestProvider;
        public ControlSmartHomeService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
     
        public async Task<bool> ControlHome(int id)    
        {
            bool result = false;
            try
            {
                string url = $"{ApiUri.POST_UPDATE_PAPER_REGISTRATION}";
                var response = await _requestProvider.PutAsync<int, ResponseBase< bool> >(url, id);
                if (response != null)
                {
                    result = response.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}
