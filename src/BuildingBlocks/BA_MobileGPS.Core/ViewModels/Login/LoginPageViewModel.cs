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
            var token = "OXomymO1njVtj0nl6lipx7jDENcy-EZdFxmq6ykmh4mgVOD0p-xm_3RDq1wd7oIbcaHO9w1GCr1vC-v2qwxef7-RZY7z9TYZ4ZYR7FVMLdc09l56W3L0CyF2_g7oZnUYw06yJKGyK-1oBgqzCaihKCU9OW0wY8wvduA4jHrAh27yS1nyIJiaDcOM5s0P8XowqSlKQ_ulecgTg7F01-JkWuXwD4bJd-DWQgDTMCPxQx_gNy1vqld5BksdWblKDD88JTpxggnND52AGvu6S5b8gVLOWnYymOxaJ9tB_pUmd4GPJHqbQY4cqFDgXiNAKlHTBqe1rDqGKupVo4uMg3fTkyi1EsAyNflf9wZka6c78ZYziPTOYeF6U6Cs6eTo2Ly_Qr6LFq-JRJ6pEum_SJkx_1m889TOQXzsCwKUAfaY-KDby9wX9Mrn7gbstBFj8Ce4uvDLv6jynky98m27eaAcOtxvhdpW9We8T4EIMnzDkeQT7QvaIwtEGvet8b8eRNegLKx5KK3tUGvAsWB5NpdtWA9MeZ0YkZyeFXJDAanpjAEd4GF6Xdi9eq8L0EIADMrSbS2nBdQCIcpr4rDJfpIdWQwk5CDGZOjCvogRH_xD1KKuK7vxkc5IG19AcK09bynB0ueWTtAmQK_MaHqAdI7Veg";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }


    }
}
