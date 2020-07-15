using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class SignalLossServices : ServiceBase<SignalLossRequest, SignalLossResponse>, ISignalLossServices
    {
        public override async Task<IList<SignalLossResponse>> GetData(SignalLossRequest input)
        {
            var respone = new List<SignalLossResponse>();
            try
            {
                var temp = await RequestProvider.PostAsync<SignalLossRequest, BaseResponse<List<SignalLossResponse>>>(ApiUri.GET_SIGNALLOSS, input);
                if (temp != null)
                {
                    if (temp.Success)
                    {
                        respone = temp.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }
    }
}
