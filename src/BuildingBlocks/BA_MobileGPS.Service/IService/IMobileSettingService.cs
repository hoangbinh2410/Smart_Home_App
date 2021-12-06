using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RealmEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IMobileSettingService : IRealmBaseService<PartnersConfig, PartnersConfiguration>
    {
        Task<MobileVersionModel> GetMobileVersion(string operatingSystem, int appID);

        Task<List<MobileConfiguration>> GetAllMobileConfigs(AppType appTypes);

        Task<PartnersConfiguration> GetPartnerConfigByCompanyID(int companyID);
    }
}