using BA_MobileGPS.Core.Resources;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginFailedPopup : PopupPage
    {
        public LoginFailedPopup()
        {
            InitializeComponent();
            btnForgotPassword.Text = MobileResource.ForgotPassword_Label_TilePage;
            btnForgotAccount.Text = MobileResource.ForgotAccount_Label_TilePage;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}