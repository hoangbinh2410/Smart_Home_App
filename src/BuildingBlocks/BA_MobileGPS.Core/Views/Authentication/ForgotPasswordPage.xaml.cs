using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
            hotline.Text = MobileSettingHelper.HotlineGps;
        }
    }
}
