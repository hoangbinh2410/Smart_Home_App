using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
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