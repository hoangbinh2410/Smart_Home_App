using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.ResponeEntity.Camera;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
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

        public async Task<List<CaptureImageData>> GetListCaptureImage(GetListImageInfoQuery request)
        {
            var result = new List<CaptureImageData>();
            try
            {
                string url = $"{ApiUri.GET_CAMERAIMAGE}";
                var response = await requestProvider.PostAsync<GetListImageInfoQuery, CaptureImageResponse>(url, request);
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