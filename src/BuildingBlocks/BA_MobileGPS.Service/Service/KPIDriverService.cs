using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class KPIDriverService : IKPIDriverService
    {
        private readonly IRequestProvider requestProvider;

        public KPIDriverService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<DriverRankingRespone>> GetDriverRankingByDay(DriverRankingRequest request)
        {
            var result = new List<DriverRankingRespone>();
            try
            {
                string url = $"{ApiUri.GET_DEVICESINFO}";
                var respone = await requestProvider.PostAsync<DriverRankingRequest, ResponseBaseV2<List<DriverRankingRespone>>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<List<DriverRankingRespone>> GetDriverRankingByMonth(DriverRankingRequest request)
        {
            var result = new List<DriverRankingRespone>();
            try
            {
                string url = $"{ApiUri.GET_DEVICESINFO}";

                var respone = await requestProvider.PostAsync<DriverRankingRequest, ResponseBaseV2<List<DriverRankingRespone>>>(url, request);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}