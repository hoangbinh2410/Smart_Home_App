using BA_MobileGPS.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public interface IHomeService
    {
        Task<List<MenuItemRespone>> GetHomeMenuAsync(int appID, string Culture);

        Task<bool> SaveConfigMenuAsync(MenuConfigRequest menuConfig);
    }
}