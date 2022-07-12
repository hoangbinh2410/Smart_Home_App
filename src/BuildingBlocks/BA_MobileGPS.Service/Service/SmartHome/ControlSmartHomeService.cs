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
    public class ControlSmartHomeService : IControlSmartHomeService
    {
        private readonly IRequestProvider _requestProvider;
        public ControlSmartHomeService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<bool> ControlAir(AirControll temp)
        {
            bool result = false;
            try
            {
                string url = string.Format("http://192.168.0.104:8000/api/v1/config/9/");
                var response = await _requestProvider.PutAsync<AirControll,ControlResponse>(url,temp);
                if (response != null)
                {
                    result = response.data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> ControlHome(int id)    
        {
            bool result = false;
            try
            {
                string url = string.Format("http://192.168.0.104:8000/api/v1/power-switch/{0}/", id);
                var response = await _requestProvider.PutAsync<ControlResponse>(url);
                if (response!=null)
                {
                    result = response.data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> ControlLight(List<Light> list)
        {
            bool result = false;
            try
            {
                string url = string.Format("http://192.168.0.104:8000/api/v1/light-switch/");
                var response = await _requestProvider.PutAsync< List<Light>,ControlResponse>(url,list);
                if (response != null)
                {
                    result = response.data;
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
