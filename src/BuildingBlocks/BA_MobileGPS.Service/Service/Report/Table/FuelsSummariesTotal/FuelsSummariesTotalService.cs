using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class FuelsSummariesTotalService : ServiceBase<FuelsSummariesTotalRequest, FuelsSummariesTotalResponse>, IFuelsSummariesTotalService
    {
        public FuelsSummariesTotalService()
        {
        }

        public async override Task<IList<FuelsSummariesTotalResponse>> GetData(FuelsSummariesTotalRequest input)
        {
            var respone = new List<FuelsSummariesTotalResponse>();
            try
            {
                var temp = await RequestProvider.PostAsync<FuelsSummariesTotalRequest, ResponseBase<List<FuelsSummariesTotalResponse>>>(ApiUri.GET_FUELSSUMMARIESTOTAL, input);
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