using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service
{
    public class ReportQCVN31SpeedService : ServiceBase<ReportQCVN31SpeedRequest, ReportQCVN31SpeedRespone>, IReportQCVN31SpeedService
    {
        public ReportQCVN31SpeedService()
        {
        }

        public async override Task<IList<ReportQCVN31SpeedRespone>> GetData(ReportQCVN31SpeedRequest input)
        {
            var respone = new List<ReportQCVN31SpeedRespone>();
            try
            {
                var temp = await RequestProvider.PostAsync<ReportQCVN31SpeedRequest, ResponseBaseV2<List<ReportQCVN31SpeedRespone>>>(ApiUri.GET_GetQCVN31SpeedReport, input);
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