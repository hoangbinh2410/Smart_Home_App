using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Device

        public async Task<StreamDevice> GetDevicesInfo(StreamDeviceRequest request)
        {
            var result = new StreamDevice();
            try
            {
                string url = $"{ApiUri.GET_DEVICESINFO}";
                var respone = await requestProvider.PostAsync<StreamDeviceRequest, ResponseStreamBase<List<StreamDevice>>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data.First();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<ResponseStreamBase<List<CameraStartRespone>>> DevicesStart(CameraStartRequest request)
        {
            var result = new ResponseStreamBase<List<CameraStartRespone>>();
            try
            {
                string url = $"{ApiUri.POST_DEVICESTART}";
                var respone = await requestProvider.PostAsync<CameraStartRequest, ResponseStreamBase<List<CameraStartRespone>>>(url, request);
                if (respone != null)
                {
                    result = respone;
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

        public async Task<ResponseStreamBase<List<PlaybackStartRespone>>> StartPlayback(PlaybackStartRequest request)
        {
            var result = new ResponseStreamBase<List<PlaybackStartRespone>>();
            try
            {
                string url = $"{ApiUri.POST_PLAYBACKSTART}";
                var respone = await requestProvider.PostAsync<PlaybackStartRequest, ResponseStreamBase<List<PlaybackStartRespone>>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone;
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

        public async Task<bool> UploadToServerStart(UploadStartRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.POST_UPLOADSTART}";
                var respone = await requestProvider.PostAsync<UploadStartRequest, ResponseStreamBase<bool>>(url, request);
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

        #endregion Device

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

        public async Task<List<CameraRestreamUploadInfo>> GetListVideoOnCloud(CameraRestreamRequest request)
        {
            var result = new List<CameraRestreamUploadInfo>();
            try
            {
                string url = $"{ApiUri.POST_RESTREAM_LISTUPLOAD}";
                var response = await requestProvider.PostAsync<CameraRestreamRequest, BaseResponse<List<CameraRestreamUploadInfo>>>(url, request);
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

        public async Task<VideoRestreamInfo> GetListVideoNotUpload(CameraUploadRequest request)
        {
            var result = new VideoRestreamInfo();
            try
            {
                string url = $"{ApiUri.POST_LISTVIDEONOTUPLOAD}";
                var response = await requestProvider.PostAsync<CameraUploadRequest, ResponseBaseV2<VideoRestreamInfo>>(url, request);
                if (response != null && response.Data != null)
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

        public async Task<List<RestreamChartData>> GetVehiclesChartDataByDate(CameraRestreamRequest request)
        {
            var result = new List<RestreamChartData>();
            try
            {
                string url = $"{ApiUri.POST_CHART_DATA}";
                var response = await requestProvider.PostAsync<CameraRestreamRequest, RestreamChartDataResponse>(url, request);
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

        public async Task<List<RestreamVideoTimeInfo>> GetListVideoPlayback(CameraPlaybackInfoRequest request)
        {
            var result = new List<RestreamVideoTimeInfo>();
            try
            {
                if (request.Channel == 0)
                {
                    request.Channel = null;
                }
                string url = $"{ApiUri.POST_LISTPLAYBACKINFO}";
                var respone = await requestProvider.PostAsync<CameraPlaybackInfoRequest, ResponseStreamBase<List<RestreamVideoTimeInfo>>>(url, request);
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

        public async Task<PackageBACameraRespone> GetPackageByXnPlate(PackageBACameraRequest request)
        {
            var result = new PackageBACameraRespone();
            try
            {
                string url = $"{ApiUri.POST_GetPACKETBYXNPLATE}";
                var respone = await requestProvider.PostAsync<PackageBACameraRequest, ResponseBaseV2<PackageBACameraRespone>>(url, request);
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

        public async Task<bool> InsertLogVideo(SaveVideoByUserRequest request)
        {
            var result = false;
            try
            {
                string url = $"{ApiUri.INSERT_LOG_VIDEO}";
                var respone = await requestProvider.PostAsync<SaveVideoByUserRequest, ResponseBaseV2<bool>>(url, request);
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

        public async Task<List<VehicleCamera>> GetListVehicleHasCamera(int xncode)
        {
            var respone = new List<VehicleCamera>();
            try
            {
                string url = string.Format(ApiUri.GET_LISTVEHICLECAMERA + "?type={0}&xnCode={1}", (int)ConditionType.MXN, xncode);
                var result = await requestProvider.GetAsync<ResponseBaseV2<List<VehicleCamera>>>(url);
                if (result != null && result.Data != null)
                {
                    respone = result.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }
    }
}