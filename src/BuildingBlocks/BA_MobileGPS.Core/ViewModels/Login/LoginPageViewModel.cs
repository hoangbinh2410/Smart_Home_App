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
            var token = "S1mLiG71oA2JHiOF5_ynxStd5hvPj8wYO4vhIGcKPyNC9SMYcnc1BQVWK1Yt4d4NwkMmP8v9XfgMqKFiXBDNxgIepVFFpFYAUiN20ONwKLyHcrb9YeJ5R-byrtUk_U0zmn2CnU8lX4odhBwlZEy1ZvU3_OG5A4bKrRti83opo6_yDF8dqLSujJSSQkdxIJPNVKFHEwkDgORdWIePylfJ0ABlgxV47am3QLw5d-XEMNT1oaYehbVPdIeAU4S3M7n7rsyiDlu7zTXBT17cUnta_d_cs7senPlDg1Gj0aMc37VOjqFxokxXeuBMuy-Z3TUi2r-OglhngnqNs1sBFislMVDamFNM65Wd9xYDtVBN73HbO5M8HTOt53_uCsJ46HStUCT__9PaXSpmW6q58dpyUdqQXY9_PjdzaC7s6vAAIvLXmAGJ_6Zyz4GcuwGP9wHhB4TVTyW1idj1t_3IXBFWzXLaOH-f8EQA4qX7KMAwVe-0BxmvD15SG-Pcwk0I2SoPW7Gy_DSOg6Pq3N8GjtHRjIojfywBDBeR-snWHP_425o6ZhkLp6hFFyzw5W1z3BVAZVO1zEobOLkoDR1OGfSba1YXgDnVxCXC8FqNbu_cRZgqENv4rLr47bU_jB9EUyUN8iKpL4rGDY8Esvr-6nnBEQ";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }
    }
}
