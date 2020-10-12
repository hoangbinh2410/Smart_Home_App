using BA_MobileGPS.Core.Resources;
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //if (string.IsNullOrEmpty(pass.Text))
            //{
            //    pass.Focus();
            //}
            //if (string.IsNullOrEmpty(account.Text))
            //{
            //    account.Focus();
            //}
        }
    }
}