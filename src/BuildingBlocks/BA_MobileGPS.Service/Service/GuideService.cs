using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class GuideService : IGuideService
    {
        private readonly IRequestProvider _requestProvider;

        public GuideService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<List<string>> GetGuide(int type)
        {
            var respone = new List<string>();
            try
            {
                var URL = string.Format(ApiUri.GET_GUIDE + "/?type={0}", type);

                var result = await _requestProvider.GetAsync<List<string>>(URL);
                if (result != null)
                {
                    respone = result;
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