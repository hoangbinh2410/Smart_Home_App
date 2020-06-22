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
            var token = "sTfL8ykAOrceV9nOTirVghd3tjyZpyVY0ShEdF4M1yR5QJKDHnt-ML1G4kCG3o6jGN5ssJyQTFCDxiSZO7eriKE35kbBvouUW6qlspuX2Zc53z4BN43UuOqBS-xsHaoC3fr2mrEEj8vvLh3FMr-muDWLVBQJvYqL3b2rSs6R-xE35yTnNyRyZ_Odxa6MLnXh7tCqSG33_vfIugJZpdW4rjXBKpv6xLvokzvnOVxddib3WIeaxFzB-GgJWVbOs4B0OWsGFYwh7He9PWW5fHNL0-OmcLIpD9VY3yIQ4016-yMosCnGEjt9g_bD6sfjZ09sxocy2GR1aRLEFgjnReWn580OIxI9le-dGNYQUNyWSvi3w3uolkm4lqLZciq2bcCaeZPFGUa4Xb6rnduaRdLbn7mbT78YydKEHcJDpsWGOddNjHiuj7ifmG4h1QmOmBB15fs4Rsh1kb5Si-4OZ6E500mWGJ02TAp7Opi8CUH_CJT0iQsDN3LWiLanqu9TkvxRqw7rGpGr2eDseANyszelX21NeVuHo0BQSbunlbR1K0BVWyaZRli9CNy-FUMTqakppSyNKPlKfDNibvRkSlm59KSV_NlEXC48wpf4BtyOc9XP2MvlJ0MSGCYuZCj1l8Uu2DKqUgyy-FivY-RYbe2QYA";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }
    }
}
