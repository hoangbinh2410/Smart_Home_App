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
    public class StreamCameraService : IStreamCameraService
    {
        private readonly IRequestProvider requestProvider;
        public StreamCameraService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }
        public async Task<StreamDevicesResponse> GetDevicesStatus(int conditionType, string conditionValue)
        {
            var result = new StreamDevicesResponse();
            try
            {             
                string url=  string.Format(ApiUri.GET_DEVICESTREAMINFOR, conditionType, conditionValue);
                result = await requestProvider.GetAsync<StreamDevicesResponse>(url);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<StreamPingResponse> RequestMoreStreamTime(StreamPingRequest request)
        {
            var result = new StreamPingResponse();
            try
            {
                string url = $"{ApiUri.POST_GETMORETIMESTREAM}";
                result = await requestProvider.PostAsync<StreamPingRequest, StreamPingResponse>(url, request);
            }
            catch (Exception ex)
            {              
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
               
            }
            return result;
        }

        public async Task<StreamStartResponse> StartStream(StreamStartRequest request)
        {           
            var result = new StreamStartResponse();
            try
            {
                string url = $"{ApiUri.POST_READYFORSTREAM}";
                result = await requestProvider.PostAsync<StreamStartRequest, StreamStartResponse>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);

            }
            return result;
        }

        public async Task<StreamStopResponse> StopStream(StreamStopRequest request)
        {
            var result = new StreamStopResponse();
            try
            {
                string url = $"{ApiUri.POST_ENDSTREAM}";
                result = await requestProvider.PostAsync<StreamStopRequest, StreamStopResponse>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);

            }
            return result;
        }

    }
}
