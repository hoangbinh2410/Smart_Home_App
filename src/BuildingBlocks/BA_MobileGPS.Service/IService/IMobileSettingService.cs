using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IMobileSettingService
    {
        Task<MobileVersionModel> GetMobileVersion(string operatingSystem, int appID);

        Task<List<MobileConfiguration>> GetAllMobileConfigs(AppType appTypes);
    }
}
