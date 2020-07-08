using BA_MobileGPS.Core.ViewModels;

using Prism.Navigation;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class BaseNavigationPage
       : NavigationPage, INavigationPageOptions, IDestructible
    {
        public bool ClearNavigationStackOnNavigation
        {
            get { return true; }
        }

        public BaseNavigationPage()
        {
            InitializeComponent();

            SetBackButtonTitle(this, "");

            Init();
        }

        private void Init()
        {
            Pushed += BaseNavigationPage_Pushed;
        }

        public void Destroy()
        {
            Pushed -= BaseNavigationPage_Pushed;
        }

        private void BaseNavigationPage_Pushed(object sender, NavigationEventArgs e)
        {
            if (e.Page.BindingContext is ViewModelBase viewModel)
            {
                viewModel.OnPushed();
            }
        }
    }
}