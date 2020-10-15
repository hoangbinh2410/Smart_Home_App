using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentView
    {
        public Home()
        {
            InitializeComponent();
            lblHighlight.Text = MobileResource.Common_Label_HighLight;
            lblFeatures.Text = MobileResource.Common_Label_Features;
        }
        
        
    }
}