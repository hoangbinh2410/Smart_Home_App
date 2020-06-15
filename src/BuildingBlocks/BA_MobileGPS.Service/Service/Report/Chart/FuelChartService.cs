using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class FuelChartService : IFuelChartService
    {
        private readonly IRequestProvider RequestProvider;

        public FuelChartService(IRequestProvider requestProvider)
        {
            RequestProvider = requestProvider;
        }

        public FuelChartRequest Request { get; set; }

        public async Task<List<FuelChartReport>> GetData()
        {
            try
            {
                var temp = await RequestProvider.PostAsync<FuelChartRequest, FuelChartResponse>(ApiUri.GET_FUELCHART, Request);
                if (temp != null)
                {
                    if (temp.State)
                    {
                        return temp.ListFuelChartReport;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return default;
        }

        public Task<List<FuelChartReport>> GetMoreData()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }
    }
}