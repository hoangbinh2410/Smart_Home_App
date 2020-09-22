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
    public enum ConditionType
    {
        MXN = 1,
        BKS = 2,
        IMEI = 3
    }
    public class StreamCameraService : IStreamCameraService
    {
        private readonly IRequestProvider requestProvider;
        public StreamCameraService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<StreamDevicesResponse> GetDevicesStatus(ConditionType type, string value)
        {
            var result = new StreamDevicesResponse();
            try
            {
                string url = string.Format(ApiUri.GET_DEVICESTREAMINFOR + "?type={0}&value={1}", (int)type, value);
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

        public async Task<List<CaptureImageData>> GetCaptureImageLimit(string xncode, string vehiclePlate, string limit)
        {
            var result = new List<CaptureImageData>();
            try
            {
                string url = string.Format(ApiUri.GET_IMAGESLIMIT + "?xncode={0}&vehiclePlate={1}&limit={2}", xncode, vehiclePlate, limit);
                var response = await requestProvider.GetAsync<ResponseStreamBase<List<CaptureImageData>>>(url);
                if (response != null && response.Data.Count > 0)
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

        public async Task<List<CaptureImageData>> GetCaptureImageTime(string xncode, string vehiclePlate, DateTime fromTime, DateTime toTime)
        {
            var result = new List<CaptureImageData>();
            try
            {
                string url = string.Format(ApiUri.GET_IMAGESTIME + "?xncode={0}&vehiclePlate={1}&fromTime={2}&toTime={3}", xncode, vehiclePlate, fromTime, toTime);
                var response = await requestProvider.GetAsync<ResponseStreamBase<List<CaptureImageData>>>(url);
                if (response != null && response.Data.Count > 0)
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
