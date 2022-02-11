using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MachineVehicleService : ServiceBase<MachineVehcleRequest, MachineVehicleResponse>, IMachineVehicleService
    {
        public MachineVehicleService()
        {
        }

        public async override Task<IList<MachineVehicleResponse>> GetData(MachineVehcleRequest input)
        {
            var respone = new List<MachineVehicleResponse>();
            try
            {
                var temp = await RequestProvider.PostAsync<MachineVehcleRequest, ResponseBaseV2<List<MachineVehicleResponse>>>(ApiUri.GET_MACHINEVEHICLE, input);
                if (temp != null && temp.Data != null)
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