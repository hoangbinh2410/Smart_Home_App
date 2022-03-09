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
            var respone = new List<FuelChartReport>();
            try
            {
                var temp = await RequestProvider.PostAsync<FuelChartRequest, ResponseBase<List<FuelChartReport>>>(ApiUri.GET_FUELCHART, Request);
                if (temp != null && temp.Data.Count > 0)
                {
                    respone= temp.Data;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return respone;
        }

        public Task<List<FuelChartReport>> GetMoreData()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }

        public async Task<ValidatedReportRespone> ValidateDateTimeReport(Guid UserId,
          DateTime FromDate, DateTime ToDate)
        {
            ValidatedReportRespone respone = new ValidatedReportRespone();
            respone.State = StateValidateReport.None;

            try
            {
                string url = $"{ApiUri.GET_VALIDATEDATETIME}?UserId={UserId}&FromDate={FromDate.ToString("yyyy-MM-dd HH:mm:ss")}&ToDate={ToDate.ToString("yyyy-MM-dd HH:mm:ss")}";

                var result = await RequestProvider.GetAsync<ResponseBase<ValidatedReportRespone>>(url);
                if (result != null && result.Data != null)
                {
                    respone = result.Data;
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