using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BA_MobileGPS.Service
{
    public class HomeService : IHomeService
    {
        private readonly IRequestProvider _requestProvider;

        public HomeService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<List<MenuItemRespone>> GetHomeMenuAsync(int appID, string Culture)
        {
            List<MenuItemRespone> result = null;
            try
            {
                string url = $"{ApiUri.GET_HOME_MENU}?culture={Culture}&appID={appID}";
                var menu = await _requestProvider.GetAsync<List<MenuItemRespone>>(url);

                if (menu != null)
                {
                    result = menu;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }

        public async Task<bool> SaveConfigMenuAsync(MenuConfigRequest menuConfig)
        {
            bool result = false;
            try
            {
                string url = ApiUri.POST_SAVE_CONFIG_HOME_MENU;
                result = await _requestProvider.PostAsync<MenuConfigRequest, bool>(url, menuConfig);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            return result;
        }
    }
}