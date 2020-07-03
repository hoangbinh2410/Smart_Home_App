using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.ViewModels.Base;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private IPopupServices _popupServices;
        public LoginPageViewModel(INavigationService navigationService,IPopupServices popupServices) : base(navigationService)
        {
            _popupServices = popupServices;
            LoginCommand = new DelegateCommand(Login);
            OpenLoginFragmentCommand = new DelegateCommand(OpenLoginFragment);
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);
        }

        private void ForgotPassword()
        {
            var color = (Color)Application.Current.Resources["LightFirstOtherColor"];
            _popupServices.ShowNotificationIconPopup("Quên mật khẩu", "Để đảm bảo an toàn thông tin, Quý khách vui lòng liên hệ <strong>19006464</strong> để được cấp lại mật khẩu. ", "ic_lock.png", color, IconPosititon.Left);
        }

        private async void Login()
        {
            await SetUserPermisisonData();
            _ = NavigationService.NavigateAsync("/MainPage");
           
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenLoginFragmentCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        private async Task SetUserPermisisonData()
        {
            if (StaticSettings.User == null)
            {
                StaticSettings.User = new LoginResponse();
            }
            StaticSettings.User.Permissions = new List<int>() { 0, 1, 6, 61, 161 ,86,91,96,106,171,253};

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
            var token = "7DZh6JiXOSCOGA9zA4CZx4f9PBNWT3jpmbvEWJJ9H_FrLxO4N0fnI1TKC2jLT57AA-oVwC0JzkY7Z8Ba-fzIgWu_Xcbxww_9pqpBpIaY-hHj0nU6ArqHhEd6cICqOb3moWSDyJ94LZX7g-Ksi9L18v3TCzzT7-PzqkVkvwe6Foygs2qJ42FAVlT8hIBE-lPjevWBPnd-Q6TcdT2l4wMG-KWfs3Oixz4jl5yKkqVgRvS_gYVe6dRF9VDXnRiBWg1tnzyYwtz3mdVNMYp7bjHzQpjxiM1iRbvlu783M_etXVmD2-3eNEVfSq8BV3tY4eGd1becCzwgR2Xq8SJAXhjXPQNL4O5PCQ8qDZPWSxIvoQdc4qzR0kba7LmGnZkrO7-3lN7hJ3qK3AjuL6_wfrJ88Xta0Bcdq-veBLV222zKZf5BqU6QK3Fz3wMspLB4Dha6HrKjesgKflm63jp6uYbe98zshRI3McHpSZfoJF7pJyhdg_L0Ra3vdquhnQ02iWRlcZQCSRt6LfjDZgzvsIqGwB2UQNgu4FO4otyc3RPS518h-duT0t5lbSnuFJ2wMoMlGfojS9Y5Va57hgnYXT99XtrgRu6FIqINwHKkwJQD3aiaKKt2trng_jEooORHAYrUgNhcJiM4mOIU13mUpssHjA";
            var request = new HttpClient();
            request.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await request.GetAsync(url);
        }

        private void OpenLoginFragment()
        {
            PopupNavigation.Instance.PushAsync(new NonLoginFeaturesPopup());
        }

    }
}
