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
            if (string.IsNullOrEmpty(pass.Text))
            {
                pass.Focus();
            }
            if (string.IsNullOrEmpty(account.Text))
            {
                account.Focus();
            }
        }
    }
}