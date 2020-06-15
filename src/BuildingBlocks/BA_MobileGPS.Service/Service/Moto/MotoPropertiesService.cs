using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
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

        public async Task<BaseResponse<MotoPropertiesRespone>> GetMotoProperties(int xnCode,string vehiclePlate)
        {
            BaseResponse<MotoPropertiesRespone> respone = new BaseResponse<MotoPropertiesRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_MOTO_PROPERTIES + "?xncode={0}&vehicleplate={1}", xnCode, vehiclePlate);
                var temp = await _IRequestProvider.GetAsync<BaseResponse<MotoPropertiesRespone>>(URL);
                if (temp != null)
                {
                    if (temp.Success)
                    {
                        respone = temp;
                    }
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