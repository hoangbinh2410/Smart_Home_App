using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ReportTemperatureService : ServiceBase<TemperartureVehicleRequest, TemperatureVehicleResponse>, IReportTemperatureService
    {
        public ReportTemperatureService()
        {
        }

        public async override Task<IList<TemperatureVehicleResponse>> GetData(TemperartureVehicleRequest input)
        {
            var respone = new List<TemperatureVehicleResponse>();
            try
            {
                var temp = await RequestProvider.PostAsync<TemperartureVehicleRequest, ResponseBaseV2<List<TemperatureVehicleResponse>>>(ApiUri.GET_REPORTTEMPERATURE, input);
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