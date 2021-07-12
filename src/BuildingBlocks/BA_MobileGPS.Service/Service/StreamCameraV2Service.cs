using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service
{
    public class StreamCameraV2Service : IStreamCameraV2Service
    {
        private readonly IRequestProvider requestProvider;

        public StreamCameraV2Service(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<StreamDevice> GetDevicesInfo(StreamDeviceRequest request)
        {
            var result = new StreamDevice();
            try
            {
                string url = $"{ApiUri.GET_DEVICESINFO}";
                var respone = await requestProvider.PostAsync<StreamDeviceRequest, ResponseStreamBase<StreamDevice>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<CameraStartRespone> DevicesStart(CameraStartRequest request)
        {
            var result = new CameraStartRespone();
            try
            {
                string url = $"{ApiUri.POST_DEVICESTART}";
                var respone = await requestProvider.PostAsync<CameraStartRequest, ResponseStreamBase<CameraStartRespone>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> DevicesStop(CameraStopRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.POST_DEVICESTOP}";
                var respone = await requestProvider.PostAsync<CameraStopRequest, ResponseStreamBase<bool>>(url, request);
                if (respone != null && respone.Data)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> DevicesPing(CameraStartRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.POST_DEVICEPING}";
                var respone = await requestProvider.PostAsync<CameraStartRequest, ResponseStreamBase<bool>>(url, request);
                if (respone != null && respone.Data)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<PlaybackStartRespone> StartPlayback(PlaybackStartRequest request)
        {
            var result = new PlaybackStartRespone();
            try
            {
                string url = $"{ApiUri.POST_PLAYBACKSTART}";
                var respone = await requestProvider.PostAsync<PlaybackStartRequest, ResponseStreamBase<PlaybackStartRespone>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> StopPlayback(PlaybackStopRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.POST_PLAYBACKSTOP}";
                var respone = await requestProvider.PostAsync<PlaybackStopRequest, ResponseStreamBase<bool>>(url, request);
                if (respone != null && respone.Data)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<UploadStartRespone> UploadToServerStart(UploadStartRequest request)
        {
            var result = new UploadStartRespone();
            try
            {
                string url = $"{ApiUri.POST_UPLOADSTART}";
                var respone = await requestProvider.PostAsync<UploadStartRequest, ResponseStreamBase<UploadStartRespone>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> UploadToServerStop(UploadStopRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.POST_UPLOADSTOP}";
                var respone = await requestProvider.PostAsync<UploadStopRequest, ResponseStreamBase<bool>>(url, request);
                if (respone != null && respone.Data)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<UploadStatusRespone>> GetUploadingProgressInfor(UploadStatusRequest request)
        {
            var result = new List<UploadStatusRespone>();
            try
            {
                string url = $"{ApiUri.POST_UPLOADPROGRESS}";
                var respone = await requestProvider.PostAsync<UploadStatusRequest, ResponseStreamBase<List<UploadStatusRespone>>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> SetHotspot(SetHotspotRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.POST_HOSTSPOT}";
                var respone = await requestProvider.PostAsync<SetHotspotRequest, ResponseStreamBase<bool>>(url, request);
                if (respone != null && respone.Data)
                {
                    result = respone.Data;
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