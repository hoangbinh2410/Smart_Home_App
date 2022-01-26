using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ActivityDetailsService : ServiceBase<ActivityDetailsRequest, ActivityDetailsModel>, IActivityDetailsService
    {
        public ActivityDetailsService()
        {
        }

        public async override Task<IList<ActivityDetailsModel>> GetData(ActivityDetailsRequest input)
        {
            var respone = new List<ActivityDetailsModel>();
            try
            {
                var temp = await RequestProvider.PostAsync<ActivityDetailsRequest, ResponseBaseV2<List<ActivityDetailsModel>>>(ApiUri.GET_DETAILS, input);
                if (temp != null && temp.Data !=null)
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