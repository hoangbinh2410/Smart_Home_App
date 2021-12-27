using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Report.Station;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service.Report.Station
{

    public class StationDetailsService : ServiceBase<StationDetailsRequest, StationDetailsResponse>, IStationDetailsService
    {
        public StationDetailsService()
        {
        }

        public async override Task<IList<StationDetailsResponse>> GetData(StationDetailsRequest input)
        {
            var respone = new List<StationDetailsResponse>();
            try
            {
                var temp = await RequestProvider.PostAsync<StationDetailsRequest, BaseResponse<List<StationDetailsResponse>>>(ApiUri.GET_GetStationDetails, input);
                if (temp != null)
                {
                    if (temp.Success)
                    {
                        respone = temp.Data;
                    }
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
