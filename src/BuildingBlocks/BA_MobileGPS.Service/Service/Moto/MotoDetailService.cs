using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MotoDetailService : IMotoDetailService
    {
        private readonly IRequestProvider _IRequestProvider;

        public MotoDetailService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<ResponseBase<MotoDetailRespone>> GetMotoDetail(int xnCode, string vehiclePlate)
        {
            ResponseBase<MotoDetailRespone> respone = new ResponseBase<MotoDetailRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_MOTO_DETAIL + "?xncode={0}&vehicleplate={1}", xnCode, vehiclePlate);
                var temp = await _IRequestProvider.GetAsync<ResponseBase<MotoDetailRespone>>(URL);
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