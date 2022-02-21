using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Infrastructure.Repository;
using BA_MobileGPS.Entities.RealmEntity;
using BA_MobileGPS.Service.Utilities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class MobileSettingService : RealmBaseService<PartnersConfig, PartnersConfiguration>, IMobileSettingService
    {
        private readonly IRequestProvider requestProvider;

        public MobileSettingService(IRequestProvider requestProvider, IBaseRepository baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<MobileConfiguration>> GetAllMobileConfigs(AppType appType)
        {
            List<MobileConfiguration> result = new List<MobileConfiguration>();
            try
            {
                int appID = (int)appType;

                string uri = string.Format(ApiUri.GET_MOBILECONFIG + "/?appID={0}", appID);

                var data = await requestProvider.GetAsync<List<MobileConfiguration>>(uri);
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

        public async Task<MobileVersionModel> GetMobileVersion(string operatingSystem, int appID)
        {
            MobileVersionModel result = new MobileVersionModel();
            try
            {
                string uri = string.Format(ApiUri.GET_MOBILEVERSION + "?os={0}&appID={1}", operatingSystem, appID);

                var data = await requestProvider.GetAsync<MobileVersionModel>(uri);

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

        public async Task<PartnersConfiguration> GetPartnerConfigByCompanyID(int companyID)
        {
            PartnersConfiguration result = new PartnersConfiguration();
            //try
            //{
            //    string uri = string.Format(ApiUri.GET_PARTNERCONFIG + "/?companyID={0}", companyID);

            //    var data = await requestProvider.GetAsync<ResponseBaseV2<PartnersConfiguration>>(uri);
            //    if (data != null && data.Data != null)
            //    {
            //        result = data.Data;
            //    }
            //}
            //catch (Exception e)
            //{
            //    Logger.WriteError(MethodBase.GetCurrentMethod().Name, e);
            //}
            return result;
        }
    }
}