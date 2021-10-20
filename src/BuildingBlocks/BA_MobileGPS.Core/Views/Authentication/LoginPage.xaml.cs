using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            account.Placeholder = MobileResource.Login_Textbox_UserName;
            pass.Placeholder = MobileResource.Login_Textbox_Password;
            checkautologin.Text = MobileResource.Login_Checkbox_Autologin;
            forgotpassword.Text = MobileResource.Login_Lable_Forgotpassword;
            btnLogin.Text = MobileResource.Login_Button_Login.ToUpper();
            if (Settings.Rememberme)
            {
                textPass.EnablePasswordVisibilityToggle = false;
            }
            else
            {
                textPass.EnablePasswordVisibilityToggle = true;
            }
            if (App.AppType == AppType.Moto)
            {
                logo.HeightRequest = 70;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}