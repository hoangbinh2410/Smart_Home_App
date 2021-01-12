using BA_MobileGPS.Core.Resources;
using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class InvalidPapersPage : ContentPage,INavigationAware
    {
        public InvalidPapersPage()
        {
            InitializeComponent();          
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
