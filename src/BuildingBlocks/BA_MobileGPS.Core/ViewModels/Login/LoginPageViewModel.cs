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
            _ = NavigationService.NavigateAsync("MainPage");
           
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
            var token = "9lTdZpomSmAm3l5UTKWfvJJ5gckSvgZbb5SXhIaAKw_x2j3awBX32NNsP75MpYMQBHgjraBXWJtAU6Y3RY4OIe0XyGN7_g1NuaupkftZ4DWp2fz40l4iB0QSinrlkvaMz4oyl9kF_o401Ih0z8uUECYiakd1je5lpNlSetBJWEnzY_i5bT9yfsapOBdUFsKDwTCHeCx8U33rNtwRJnxTBnnrFiiPWdXIv6XzHyGkLjahRqS9o2mnLE3mBMyc6r2d1cEAH72_g1Y9jTDqYEPYReLQxfKjNaTPILVfhwSHjFlcCvijqoOACdVPyH4iNfXRLqBj0kljcGw3sLCxgGy0Nbcx3GxMkrUKUyVruDVZEZo08azvATJadC9JAQtd7t1Q1ecvDmUhbgQEbTnVrE-6B8stOBDlztjt7qSPZfagnwtXAB3y5gbqXHB0-nVFpNZnnKvfNJIIGXJqc38Q-2kPT7Lulu9u8YSq6n_xs1CbbSBT7izHk53a6s3PtOqZtaFRCaq9nxY9PFS_2lgk-sOPcSYqpkaykhmEzFGUGBq0lkcbSWrQmyBfm_yPCefRse3Ou3GhIH5Q0y-GPe8Xao7k2pqHHjiZCyg4fWZ3lAtS96RDBKW1JGcOO8lVb__OgJADw14g3gYOGlIue1HXnrKYDw";
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
