using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var tabIndex_0 = PrismApplicationBase.Current.Container.Resolve<View>("Index0"); //Home
            Switcher.Children.Add(new Home());
            Switcher.Children.Add(tabIndex_0);

            Switcher.SelectedIndex = 0;
        }

     
    }
}
