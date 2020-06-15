using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class HelperService : IHelperService
    {
        private readonly IRequestProvider requestProvider;

        public HelperService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<HeplerModel> GetHelper(HelperRequest request)
        {
            HeplerModel result = new HeplerModel();
            try
            {
                string url = $"{ApiUri.GET_CAMERAIMAGE}";
                result = await requestProvider.PostAsync<HelperRequest, HeplerModel>(url, request);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}