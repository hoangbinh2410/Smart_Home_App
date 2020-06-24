using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private async void Login()
        {
            await SetUserPermisisonData();
            await NavigationService.NavigateAsync("MainPage");
           
        }

        public ICommand LoginCommand { get; }

        private async Task SetUserPermisisonData()
        {
            if (StaticSettings.User == null)
            {
                StaticSettings.User = new LoginResponse();
            }
            StaticSettings.User.Permissions = new List<int>() { 0, 1, 6, 61, 161 ,86,91,96,106};

            var respose = await RequestData();
            var contentStream = await respose.Content.ReadAsStringAsync();
            var allItems = JsonConvert.DeserializeObject<List<HomeMenuItem>>(contentStream);
            var permisisons = UserInfo.Permissions.Distinct();

            StaticSettings.ListMenu = allItems.Where(x => permisisons.Contains(x.PermissionViewID) && x.MenuItemParentID != 0).ToList();
            foreach (var item in StaticSettings.ListMenu)
            {
                item.IconMobile = "http://api.bagroup.vn" + item.IconMobile;
            }
        }


        private async Task<HttpResponseMessage> RequestData()
        {
           
            var url = "http://api.bagroup.vn/api/v2/menu/getmenubyculture?culture=vi-vn&appID=0";
            var token = "Wk5-AKQMk0P2TRZ8wcJn9WaZ8I_NLg_-9Np-8eu4EZiKwr6-1YcEM07Ap5zb9i0YJKXXqhoL8CmMu8D4EATLxyoVDX01vwzGC4USxQk7CZI0PNJ99qtSmXGuvVF6Diq-VF_7yHNOzdO9N7pffgVBj0UGYYGLuR1if-jy2Y4A48ATur3VnhV9_RvAxIoxyZI7anM6Vplo_P-2UfBUr58C24O2aWgMtTdAPj83NLwSk9DuG701epr9xVcRjrMc7WKOqMDAYDl0NQUUaiFyKrNYCSH97U3gmdRzZH0IEQDgJ2v5BBFX7nIxpMFgvOUCP5uEMFcdrfyrYX-w-tJv_YZE9m2QpSjS2JRmQozwAezIXGzNhPDjzp53iYCe4cV6yJLrBCwzRwkUygkAFCf9DF7TVb09aOCElp0if3fWg89ZY2KQUmp78YlWFPvPzjXIbpjWuuJ7vXP7_3F_quvBThM96mIpBmhd0_JQvDAb3MRMUmddLjpmhvQH7WhsTSHlFj4bzuHeS1MZjRUFlIvEG0tZ5R-4L2EzHww2cGJ5Mj8df-cX47scPJlHPtt4B1MTI3wreqzhOD2wuP2rVtNX1d3Nj4S8bjQ0TeUfH5ccrlHnzVRfeDlmDe6ko3o2CAA6oVOLObcVci75VRzLUBLn4k8kGg";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }

        private void SetTabItemMenu()
        {

        }
    }
}
