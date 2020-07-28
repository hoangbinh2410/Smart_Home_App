using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();         
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.IsFirstLoadLogin)
            {
                account.Text = string.Empty;
                pass.Text = string.Empty;
                Settings.IsFirstLoadLogin = false;

            }
        }
    }
}
