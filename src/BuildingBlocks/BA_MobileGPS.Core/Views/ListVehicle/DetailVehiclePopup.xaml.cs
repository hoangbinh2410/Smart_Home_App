using BA_MobileGPS.Core.Resources;
using Rg.Plugins.Popup.Pages;

namespace BA_MobileGPS.Core.Views
{
    public partial class DetailVehiclePopup : PopupPage
    {
        public DetailVehiclePopup()
        {
            InitializeComponent();
            lblOnlineTitle.Text = MobileResource.Online_Label_TitlePage;
            lblRouteTitle.Text = MobileResource.Route_Label_Title;
            lblDetailTitle.Text = MobileResource.DetailVehicle_Label_TilePage;
        }
    }
}