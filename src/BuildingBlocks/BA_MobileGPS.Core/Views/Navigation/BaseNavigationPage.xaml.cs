using Prism.Navigation;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class BaseNavigationPage
       : NavigationPage, INavigationPageOptions
    {
        public bool ClearNavigationStackOnNavigation
        {
            get { return true; }
        }

        public BaseNavigationPage()
        {
            InitializeComponent();

            SetBackButtonTitle(this, "");
        }
    }
}