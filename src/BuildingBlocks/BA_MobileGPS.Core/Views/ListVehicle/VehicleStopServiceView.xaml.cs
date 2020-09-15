using BA_MobileGPS.Core.Resources;
using Sharpnado.MaterialFrame;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleStopServiceView : MaterialFrame
    {
        public VehicleStopServiceView()
        {
            InitializeComponent();
            lblStoppingDate.Text = MobileResource.ListVehicle_Label_Vehicle_Stop_Date;
        }
    }
}