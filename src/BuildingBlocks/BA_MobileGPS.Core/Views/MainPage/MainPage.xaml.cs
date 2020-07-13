using Prism;
using Prism.Ioc;
using Prism.Mvvm;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var tabIndex_0 = PrismApplicationBase.Current.Container.Resolve<ContentView>("Index0"); //Home
            ViewModelLocator.SetAutowirePartialView(tabIndex_0, MainContentPage);
            Switcher.Children.Add(new Home());
            Switcher.Children.Add(tabIndex_0);// Trang online
            Switcher.Children.Add(new Home());
            var tabIndex_4 = PrismApplicationBase.Current.Container.Resolve<ContentView>("Index4"); //Account
            ViewModelLocator.SetAutowirePartialView(tabIndex_4, MainContentPage);
            Switcher.Children.Add(tabIndex_4);

            Switcher.SelectedIndex = 0;
        }

     
    }
}
