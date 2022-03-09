using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class SpeedOversService : ServiceBase<SpeedOversRequest, SpeedOversModel>, ISpeedOversService
    {
        public SpeedOversService()
        {
        }

        public override async Task<IList<SpeedOversModel>> GetData(SpeedOversRequest input)
        {
            var respone = new List<SpeedOversModel>();
            try
            {
                var temp = await RequestProvider.PostAsync<SpeedOversRequest, ResponseBase<List<SpeedOversModel>>>(ApiUri.GET_SPEEDOVERS, input);
                if (temp != null && temp.Data.Count > 0)
                {
                    respone = temp.Data;
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