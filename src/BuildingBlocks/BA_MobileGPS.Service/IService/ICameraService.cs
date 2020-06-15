using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity.Camera;

using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ICameraService
    {
        Task<CameraImageResponse> GetCamera(CameraImageRequest request);
    }
}