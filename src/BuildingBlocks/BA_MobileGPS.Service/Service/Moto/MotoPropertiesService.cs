using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MotoPropertiesService : IMotoPropertiesService
    {
        private readonly IRequestProvider _IRequestProvider;

        public MotoPropertiesService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<ResponseBase<MotoPropertiesRespone>> GetMotoProperties(int xnCode, string vehiclePlate)
        {
            ResponseBase<MotoPropertiesRespone> respone = new ResponseBase<MotoPropertiesRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_MOTO_PROPERTIES + "?xncode={0}&vehicleplate={1}", xnCode, vehiclePlate);
                var temp = await _IRequestProvider.GetAsync<ResponseBase<MotoPropertiesRespone>>(URL);
                if (temp != null && temp.Data != null)
                {
                    respone = temp;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return respone;
        }
    }
}