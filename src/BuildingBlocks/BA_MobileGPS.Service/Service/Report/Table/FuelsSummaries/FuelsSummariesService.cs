using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class FuelsSummariesService : ServiceBase<FuelsSummariesRequest, FuelsSummariesModel>, IFuelsSummariesService
    {
        public FuelsSummariesService()
        {
        }

        public async override Task<IList<FuelsSummariesModel>> GetData(FuelsSummariesRequest input)
        {
            var respone = new List<FuelsSummariesModel>();
            try
            {
                var temp = await RequestProvider.PostAsync<FuelsSummariesRequest, ResponseBaseV2<List<FuelsSummariesModel>>>(ApiUri.GET_FUELSSUMMARIES, input);
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