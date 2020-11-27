using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MotoSimMoneyService : IMotoSimMoneyService
    {
        private readonly IRequestProvider _IRequestProvider;

        public MotoSimMoneyService(IRequestProvider IRequestProvider)
        {
            this._IRequestProvider = IRequestProvider;
        }

        public async Task<BaseResponse<SimMoneyRespone>> GetSimMoney(long vehicleID)
        {
            BaseResponse<SimMoneyRespone> respone = new BaseResponse<SimMoneyRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_SIM_MONEY + "?vehicleID={0}", vehicleID);
                var temp = await _IRequestProvider.GetAsync<BaseResponse<SimMoneyRespone>>(URL);
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