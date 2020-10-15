using BA_MobileGPS.Core.Resources;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListVehicleHelpPage : PopupPage
    {
        public ListVehicleHelpPage()
        {
            InitializeComponent();
            lblNote.Text = MobileResource.ListVehicle_Label_Note;
            lblVehicleNormal.Text = MobileResource.ListVehicle_Label_VehicleNormal2VMS;
            lblVehicleStop.Text = MobileResource.ListVehicle_Label_VehicleStop2VMS;
            lblVehicleSpeeding.Text = MobileResource.ListVehicle_Label_VehicleSpeeding2VMS;
            lblVehicleLostGPS.Text = MobileResource.ListVehicle_Label_VehicleLostGPS2VMS;
            lblVehicleLostGSM.Text = MobileResource.ListVehicle_Label_VehicleLostGSM2VMS;
        }
    }
}