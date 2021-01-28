using BA_MobileGPS.Core.Controls;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class ListPapersPage : ContentPage,INavigationAware
    {
        public ListPapersPage()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            foreach (var item in tabview.Items)
            {
                if (item.IsSelected)
                {
                    PageUtilities.OnNavigatedFrom(item.Content, parameters);
                }
            }
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            foreach (var item in tabview.Items)
            {
                if (item.IsSelected)
                {
                    PageUtilities.OnNavigatedTo(item.Content, parameters);
                }
            }
        }
    }
}
