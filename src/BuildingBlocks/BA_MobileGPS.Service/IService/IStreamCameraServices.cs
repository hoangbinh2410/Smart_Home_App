using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IStreamCameraService
    {
        #region Device
        Task<StreamDevice> GetDevicesInfo(StreamDeviceRequest request);

        Task<ResponseStreamBase<List<CameraStartRespone>>> DevicesStart(CameraStartRequest request);

        Task<bool> DevicesStop(CameraStopRequest request);

        Task<bool> DevicesPing(CameraStartRequest request);

        Task<ResponseStreamBase<List<PlaybackStartRespone>>> StartPlayback(PlaybackStartRequest request);

        Task<bool> StopPlayback(PlaybackStopRequest request);

        Task<bool> StopAllPlayback(PlaybackStopRequest request);

        Task<bool> UploadToServerStart(UploadStartRequest request);

        Task<bool> UploadToServerStop(UploadStopRequest request);

        Task<List<UploadStatusRespone>> GetUploadingProgressInfor(UploadStatusRequest request);

        Task<bool> SetHotspot(SetHotspotRequest request);
        #endregion

        Task<List<CaptureImageData>> GetCaptureImageLimit(int xncode, string vehiclePlate, int limit);

        Task<List<CaptureImageData>> GetListCaptureImage(StreamImageRequest request);

        Task<List<CameraRestreamUploadInfo>> GetListVideoOnCloud(CameraRestreamRequest request);

        Task<VideoRestreamInfo> GetListVideoNotUpload(CameraUploadRequest request);

        Task<List<RestreamChartData>> GetVehiclesChartDataByDate(CameraRestreamRequest request);

        Task<List<RestreamVideoTimeInfo>> GetListVideoPlayback(CameraPlaybackInfoRequest request);

        Task<PackageBACameraRespone> GetPackageByXnPlate(PackageBACameraRequest request);

        Task<bool> InsertLogVideo(SaveVideoByUserRequest request);

        Task<List<VehicleCamera>> GetListVehicleHasCamera(int xncode);
    }

    public enum ConditionType
    {
        MXN = 1,
        BKS = 2,
        IMEI = 3
    }
}