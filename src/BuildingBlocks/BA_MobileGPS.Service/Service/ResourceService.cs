using AutoMapper;

using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class ResourceService : RealmBaseService<MobileResourceRealm, MobileResourceRespone>, IResourceService
    {
        private readonly IRequestProvider RequestProvider;

        public ResourceService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            this.RequestProvider = requestProvider;
        }

        public async Task<List<MobileResourceRespone>> GetAllResources(int appID, string Culture, long? LastTime)
        {
            List<MobileResourceRespone> result = new List<MobileResourceRespone>();
            try
            {
                var url = $"{ApiUri.GET_MOBILERESOURCE}?appID={appID}&culture={Culture}&LastUpdate={LastTime}";
                var data = await RequestProvider.GetAsync<List<MobileResourceRespone>>(url);

                if (data != null)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }

        public async Task<List<MobileConfiguration>> GetAllMobileConfigs(AppType appType)
        {
            List<MobileConfiguration> result = new List<MobileConfiguration>();
            try
            {
                int appID = (int)appType;

                string uri = string.Format(ApiUri.GET_MOBILECONFIG + "/?appID={0}", appID);

                var data = await RequestProvider.GetAsync<List<MobileConfiguration>>(uri);
                if (data != null && data.Count > 0)
                {
                    result = data;
                }
            }
            catch (Exception e)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            }
            return result;
        }
    }
}