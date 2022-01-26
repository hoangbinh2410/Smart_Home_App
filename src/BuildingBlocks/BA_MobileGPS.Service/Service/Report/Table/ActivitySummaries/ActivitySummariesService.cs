using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ActivitySummariesService : ServiceBase<ActivitySummariesRequest, ActivitySummariesModel>, IActivitySummariesService
    {
        public ActivitySummariesService()
        {
        }

        public async override Task<IList<ActivitySummariesModel>> GetData(ActivitySummariesRequest input)
        {
            var respone = new List<ActivitySummariesModel>();
            try
            {
                var temp = await RequestProvider.PostAsync<ActivitySummariesRequest, ResponseBaseV2<List<ActivitySummariesModel>>>(ApiUri.GET_ACTIVITYSUMMARIES, input);
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