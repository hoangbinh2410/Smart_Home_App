using BA_MobileGPS.Core.Resources;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPopup : PopupPage
    {
        public ForgotPasswordPopup()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var color = ((Color)Application.Current.Resources["PrimaryColor"]).ToHex().Replace("FF",string.Empty);
            content.Text = string.Format(MobileResource.Login_ForgotPassword_PopupContent, color, MobileSettingHelper.HotlineGps);
        }
    }
}