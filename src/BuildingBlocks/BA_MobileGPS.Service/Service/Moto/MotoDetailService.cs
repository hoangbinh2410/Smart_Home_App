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

        public async Task<BaseResponse<MotoDetailRespone>> GetMotoDetail(int xnCode, string vehiclePlate)
        {
            BaseResponse<MotoDetailRespone> respone = new BaseResponse<MotoDetailRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_MOTO_DETAIL + "?xncode={0}&vehicleplate={1}", xnCode, vehiclePlate);
                var temp = await _IRequestProvider.GetAsync<BaseResponse<MotoDetailRespone>>(URL);
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