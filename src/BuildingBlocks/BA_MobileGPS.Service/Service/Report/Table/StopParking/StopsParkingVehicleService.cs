using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class StopsParkingVehicleService : ServiceBase<StopParkingVehicleRequest, StopParkingVehicleModel>, IStopParkingVehicleService
    {
        public StopsParkingVehicleService()
        {
        }

        public override async Task<IList<StopParkingVehicleModel>> GetData(StopParkingVehicleRequest input)
        {
            var respone = new List<StopParkingVehicleModel>();
            try
            {
                var temp = await RequestProvider.PostAsync<StopParkingVehicleRequest, ResponseBase<List<StopParkingVehicleModel>>>(ApiUri.GET_STOPPARKING, input);
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