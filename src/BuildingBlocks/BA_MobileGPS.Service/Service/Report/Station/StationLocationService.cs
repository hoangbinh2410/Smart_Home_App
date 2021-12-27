using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity.Report.Station;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service.Service.Report.Station
{
    public class StationLocationService : IStationLocationService
    {
        private readonly IRequestProvider _iRequestProvider;

        public StationLocationService(IRequestProvider iRequestProvider)
        {
            this._iRequestProvider = iRequestProvider;
        }

        public async Task<List<LocationStationResponse>> GetListLocationStation(int id)
        {
            List<LocationStationResponse> result = new List<LocationStationResponse>();
            try
            {
                string uri = string.Format(ApiUri.GET_GetListLocationStation + "?companyID={0}", id);
                var respone = await _iRequestProvider.GetAsync<ResponseBaseV2<List<LocationStationResponse>>>(uri);
                if (respone != null && respone.Data != null)
                {
                    result = respone.Data;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}
