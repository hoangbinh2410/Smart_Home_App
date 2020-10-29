using BA_MobileGPS.Core.Resources;
using Prism.AppModel;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentView, INavigationAware
    {
        public Home()
        {
            InitializeComponent();
            lblHighlight.Text = MobileResource.Common_Label_HighLight;
            lblFeatures.Text = MobileResource.Common_Label_Features;
            var a = BindingContext;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
           
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var a = BindingContext;
        }
    }
}