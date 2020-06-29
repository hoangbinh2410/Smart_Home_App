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
            _ = NavigationService.NavigateAsync("MainPage");
           
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

            StaticSettings.ListMenuOriginGroup = allItems.Where(x => permisisons.Contains(x.PermissionViewID) && x.MenuItemParentID != 0).ToList();
            foreach (var item in StaticSettings.ListMenuOriginGroup)
            {
                item.IconMobile = "http://api.bagroup.vn" + item.IconMobile;
            }
        }


        private async Task<HttpResponseMessage> RequestData()
        {
           
            var url = "http://api.bagroup.vn/api/v2/menu/getmenubyculture?culture=vi-vn&appID=0";
            var token = "iFEp1dwf_pZvb7Ud-qlDULOh7Y0kN_0TKn8hwhjRewpl7cOYjgdKuuRBSezQYxL-0gmCBZGSm3QLPJZQOAWvclfMo91FREUoZfKScBFdo4_MaDoH5VHkQoLhKBjMQ3cLMnM2qS5UPfq2JCa-NP83DpSpQ20MIADqw-8GlSZ82_Hk6e_FUNHAnlu2KP_BL6ABCsvs8r6pLYwQFm7Kpb-26YSdnMqOoRGUoxsNvVxoS5U_EGQizjtNDuTNyu4t3Lunuv5i8tcUkhP47KUhwx_feECcz1K6SSvzCdqen9KoRpKxt2MEcz9iqYImTN5niB1i8MK1lQHUSXUIQnQviLJFeMcdbgDUVDEWmZIqZ35Op0pOZjmW3xtoZxL3jQNQpb8Ey0HjXi-RFjYWOyoSRui0-WnRTC3bn3Cc3oZ0SvmVyJMnStVvHEe1C6WQcloVQ6zhxpp3bAo0H7bf1iUZfEYC5a3hwWTjOW4SLDqR8a-Fe-10cqX3hF5HmSixwD-5OkiXfIxha0jLXWrFxO2BYJVU4tHceCTOOChRAt-mAtg_wdO9BIkQr4xbOk_EHhkGofmhou73J3aOj8N8Afi4grMtguEKjehXKf_E1B_8_6wnG90TQV-QsEPVpm-5N34y3sUU7xlMIu7l0BOnk4gylYPusQ";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }


    }
}
