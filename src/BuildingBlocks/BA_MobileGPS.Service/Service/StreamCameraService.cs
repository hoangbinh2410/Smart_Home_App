using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        public async Task<List<CaptureImageData>> GetCaptureImageLimit(int xncode, string vehiclePlate, int limit)
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

        public async Task<List<CaptureImageData>> GetCaptureImageTime(int xncode, string vehiclePlate, DateTime fromTime, DateTime toTime)
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

        public async Task<List<CaptureImageData>> GetListCaptureImage(StreamImageRequest request)
        {
            var result = new List<CaptureImageData>();
            try
            {
                string url = $"{ApiUri.GET_IMAGES}";
                var response = await requestProvider.PostAsync<StreamImageRequest, CaptureImageResponse>(url, request);
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

        public async Task<List<CameraRestreamInfo>> GetListVideoByDate(CameraRestreamRequest request)
        {
            var result = new List<CameraRestreamInfo>();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_INFOR}";
                var response = await requestProvider.PostAsync<CameraRestreamRequest, CameraRestreamInfoResponse>(url, request);
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

        public async Task<List<CameraRestreamUploadInfo>> GetListVideoOnServer(CameraRestreamRequest request)
        {
            var result = new List<CameraRestreamUploadInfo>();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_LISTUPLOAD}";
                var response = await requestProvider.PostAsync<CameraRestreamRequest, CameraRestreamUploadResponse>(url, request);
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

        public async Task<RestreamStartResponese> StartRestream(StartRestreamRequest request)
        {
            var result = new RestreamStartResponese();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_START}";
                result = await requestProvider.PostAsync<StartRestreamRequest, RestreamStartResponese>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<StreamStopResponse> StopRestream(StopRestreamRequest request)
        {
            var result = new StreamStopResponse();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_STOP}";
                result = await requestProvider.PostAsync<StopRestreamRequest, StreamStopResponse>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<CaptureImageData>> RestreamCaptureImageInfo(int xncode, string vehiclePlate, DateTime fromTime, DateTime toTime, int? channel = null, int? limit = null)
        {
            var result = new List<CaptureImageData>();
            try
            {

                var from = fromTime.ToString("yyyy/MM/dd HH:mm:ss").Replace(" ", "T"); 
                var to = toTime.ToString("yyyy/MM/dd HH:mm:ss").Replace(" ", "T");
                string url = string.Format(ApiUri.GET_RESTREAM_IMAGES + "?xncode={0}&vehiclePlate={1}&fromTime={2}&toTime={3}&limit={4}&channel={5}", xncode, vehiclePlate, from, to,limit,channel);
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

        public async Task<RestreamUploadResponse> UploadToCloud(StartRestreamRequest request)
        {
            var result = new RestreamUploadResponse();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_UPLOAD}";
                result = await requestProvider.PostAsync<StartRestreamRequest, RestreamUploadResponse>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<RestreamUploadResponse> CancelUploadToCloud(StopRestreamRequest request)
        {
            var result = new RestreamUploadResponse();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_CANCELUPLOAD}";
                result = await requestProvider.PostAsync<StopRestreamRequest, RestreamUploadResponse>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}