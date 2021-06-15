using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.IService
{
    public interface IStreamCameraService
    {
        Task<StreamStartResponse> StartStream(StreamStartRequest request);

        Task<StreamStopResponse> StopStream(StreamStopRequest request);

        Task<StreamPingResponse> RequestMoreStreamTime(StreamPingRequest request);

        /// <summary>
        /// </summary>
        /// <param name="conditionType">Kiểu tìm kiếm
        /// 1: Tìm theo MXN
        /// 2: Tìm theo BKS
        /// 3: Tìm theo IMEI</param>
        /// <param name="conditionValue">Thông tin tìm kiếm</param>
        /// <returns></returns>
        Task<StreamDevicesResponse> GetDevicesStatus(ConditionType type, string value);

        Task<StreamDevicesResponse> GetListVehicleCamera(int xncode);

        Task<List<CaptureImageData>> GetCaptureImageLimit(int xncode, string vehiclePlate, int limit);

        Task<List<CaptureImageData>> GetCaptureImageTime(int xncode, string vehiclePlate, DateTime fromTime, DateTime toTime);

        /// <summary>
        /// tra ve 4 anh gan nhat
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<CaptureImageData>> GetListCaptureImage(StreamImageRequest request);

        Task<List<CameraRestreamInfo>> GetListVideoOnDevice(CameraRestreamRequest request);

        Task<List<CameraRestreamUploadInfo>> GetListVideoOnCloud(CameraRestreamRequest request);

        Task<VideoRestreamInfo> GetListVideoNotUpload(CameraUploadRequest request);

        Task<RestreamStartResponese> StartRestream(StartRestreamRequest request);

        Task<StreamStopResponse> StopRestream(StopRestreamRequest request);

        Task<List<CaptureImageData>> RestreamCaptureImageInfo(int xncode, string vehiclePlate, DateTime fromTime, DateTime toTime, int? channel = null, int? limit = null);

        Task<RestreamUploadResponse> UploadToCloud(StartRestreamRequest request);

        Task<RestreamUploadResponse> CancelUploadToCloud(StopRestreamRequest request);

        Task<UploadProgressResponse> GetUploadProgress(int xncode, string vehiclePlate, int channel);

        Task<List<RestreamChartData>> GetVehiclesChartDataByDate(CameraRestreamRequest request);

        Task<List<RestreamVideoTimeInfo>> DeviceTabGetVideoInfor(int xncode, string vehiclePlate, DateTime fromTime, DateTime toTime, int? channel = null);

        Task<PackageBACameraRespone> GetPackageByXnPlate(PackageBACameraRequest request);

        Task<bool> InsertLogVideo(SaveVideoByUserRequest request);

        Task<bool> SetHotspot(int xncode, string vehiclePlate, int state);

        Task<List<VehicleCamera>> GetListVehicleHasCamera(int xncode);
    }

    public enum ConditionType
    {
        MXN = 1,
        BKS = 2,
        IMEI = 3
    }
}