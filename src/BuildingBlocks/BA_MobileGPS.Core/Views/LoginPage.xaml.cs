using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class LoginPage : ContentPage
    {
        private INavigationService _navigation;
        public LoginPage(INavigationService navigation)
        {
            InitializeComponent();
            _navigation = navigation;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            _navigation.NavigateAsync("HomePage");
        }
    }
}
