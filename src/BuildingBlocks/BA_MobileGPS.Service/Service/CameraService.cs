using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class CameraService : ICameraService
    {
        private readonly IRequestProvider requestProvider;

        public CameraService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<CameraImageResponse> GetCamera(CameraImageRequest request)
        {
            CameraImageResponse result = new CameraImageResponse();
            try
            {
                string url = $"{ApiUri.GET_CAMERAIMAGE}";
                result = await requestProvider.PostAsync<CameraImageRequest, CameraImageResponse>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }

            return result;
        }
    }
}