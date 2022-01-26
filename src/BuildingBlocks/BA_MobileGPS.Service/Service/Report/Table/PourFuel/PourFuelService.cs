using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class PourFuelService : ServiceBase<FuelReportRequest, FuelVehicleModel>, IPourFuelService
    {
        public PourFuelService()
        {
        }

        public async override Task<IList<FuelVehicleModel>> GetData(FuelReportRequest input)
        {
            var respone = new List<FuelVehicleModel>();
            try
            {
                var temp = await RequestProvider.PostAsync<FuelReportRequest, ResponseBaseV2<List<FuelVehicleModel>>>(ApiUri.GET_FUELVEHICLE, input);
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