using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class GeocodeService : IGeocodeService
    {
        private readonly IRequestProvider requestProvider;

        public GeocodeService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<string> GetAddressByLatLng(int companyID, string lat, string lng)
        {
            var respone = string.Empty;
            try
            {
                var URL = string.Format(ApiUri.GET_GETADDRESSBYLATLNG + "?companyID={0}&lat={1}&lng={2}", companyID, lat, lng);

                var result = await requestProvider.GetAsync<ResponseBase<string>>(URL);
                if (result != null)
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

        public async Task<List<string>> GetAddressesByLatLng(string lats, string lngs)
        {
            var respone = new List<string>();
            //try
            //{
            //    var URL = string.Format(ApiUri.GET_ADDRESSESBYLATLNG + "?lat={0}&lng={1}", lats, lngs);

            //    var result = await requestProvider.GetAsync<List<string>>(URL);
            //    if (result != null)
            //    {
            //        respone = result;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            //}
            return respone;
        }
    }
}