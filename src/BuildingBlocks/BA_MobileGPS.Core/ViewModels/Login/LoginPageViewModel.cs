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
            var color = (Color)Application.Current.Resources["LightPrimaryOtherColor"];
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
            var token = "9gXmIZTdV5joZxFhUJR-j2mC3lUJisW9ZaRYyEl1vnyKYma5zuWcIOPWCt9lgn2cy9xzT7-Ph5d6ziht6mtIg4r1evaGRrQ-AhIdtVp1FWIQSVVCp1i4X02PV0GkfwVpuY9s-9Ay8rQf_0w1iyBDwcJ4h1nK08KTdbqYHKhELVQWigrwfdggMU8RI2PPCuBcZqJxOKTlSKGRbjcOwMlUlgsz89am9R8Mg-Pp-RuOvA-f2VDNd7_7OLlEyajhul-uOCYLov9ze0ehSexP-T1BOdjESewOtzM7KFfrf2Bajyy05Awa1HUefAJJ2cgbx1v6aOMsfvX4xoUr3Lfk16E3I5MW-Tlr3jLoXLG-OVX2l03AG8vAtDVWcGqRrX59yrrqt3idhjsl98jV3YjgfBISCE1NJNbSzp9GLqJhO56NfnkYvS4fv5Ry2Xgh3Unc7xganQ3EozbTH2jiFhrN8VnzlBCLa7nH6i6bpRUSIy4V0672SOL0C9t4zo_DXs3taCngJl-U4_5c_AzRCwmmHj59XCgK4eEzCYOJDhwAqi8nox5080JDpeR4sch5R4z0h5lDjNHjImKGdfgFaCFdNsAwAqDKcmVVNsoHTmdsuCRFZFdZOUzKVhU2Y9Bk9nlfDHQFrNiP25sz4Z6PXj55hoXM1g";
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
