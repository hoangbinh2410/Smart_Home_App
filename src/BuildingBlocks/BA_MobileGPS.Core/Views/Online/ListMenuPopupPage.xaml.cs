using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace BA_MobileGPS.Core.Views
{
    public partial class ListMenuPopupPage : PopupPage
    {
        public ListMenuPopupPage()
        {
            InitializeComponent();
            lblFeatures.Text = MobileResource.Common_Label_Features;
            lblReport.Text = MobileResource.Common_Label_Report;
            lblCamera.Text = MobileResource.Camera_Label_MenuTitle;
        }
    }
}