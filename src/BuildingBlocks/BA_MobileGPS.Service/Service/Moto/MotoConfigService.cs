using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MotoConfigService : IMotoConfigService
    {
        private readonly IRequestProvider _IRequestProvider;

        public MotoConfigService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<MotoConfigRespone> SendConfigMoto(MotoConfigRequest request)
        {
            MotoConfigRespone result = new MotoConfigRespone();
            try
            {
                string url = $"{ApiUri.SEND_CONFIG_MOTO}";
                var data = await _IRequestProvider.PostAsync<MotoConfigRequest, ResponseBase<MotoConfigRespone>>(url, request);
                if (data != null)
                {
                    result = data.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}