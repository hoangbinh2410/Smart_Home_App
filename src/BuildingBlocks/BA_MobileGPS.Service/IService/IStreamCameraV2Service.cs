using BA_MobileGPS.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IStreamCameraV2Service
    {
        Task<StreamDevice> GetDevicesInfo(StreamDeviceRequest request);

        Task<CameraStartRespone> DevicesStart(CameraStartRequest request);

        Task<bool> DevicesStop(CameraStopRequest request);

        Task<bool> DevicesPing(CameraStartRequest request);

        Task<PlaybackStartRespone> StartPlayback(PlaybackStartRequest request);

        Task<bool> StopPlayback(PlaybackStopRequest request);

        Task<UploadStartRespone> UploadToServerStart(UploadStartRequest request);

        Task<bool> UploadToServerStop(UploadStopRequest request);

        Task<List<UploadStatusRespone>> GetUploadingProgressInfor(UploadStatusRequest request);

        Task<bool> SetHotspot(SetHotspotRequest request);
    }
}