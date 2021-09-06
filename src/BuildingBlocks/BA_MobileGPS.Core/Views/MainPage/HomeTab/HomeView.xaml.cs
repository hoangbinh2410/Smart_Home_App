using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentView
    {
        public HomeView()
        {
            InitializeComponent();
            lblHighlight.Text = MobileResource.Common_Label_HighLight;
            lblFeatures.Text = MobileResource.Common_Label_Features;
            lblReport.Text = MobileResource.Common_Label_Report;
            lblCamera.Text = MobileResource.Camera_Label_MenuTitle;
            lbSetting.Text = MobileResource.Menu_Label_SettingFavorite;
        }
    }
}