using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface ICameraService
    {
        Task<List<CaptureImageData>> GetListCaptureImage(GetListImageInfoQuery request);
    }
}