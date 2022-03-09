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

        public async Task<ResponseBase<SimMoneyRespone>> GetSimMoney(long vehicleID)
        {
            ResponseBase<SimMoneyRespone> respone = new ResponseBase<SimMoneyRespone>();
            try
            {
                var URL = string.Format(ApiUri.GET_SIM_MONEY + "?vehicleID={0}", vehicleID);
                var temp = await _IRequestProvider.GetAsync<ResponseBase<SimMoneyRespone>>(URL);
                if (temp != null && temp.Data!=null)
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