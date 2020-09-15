using BA_MobileGPS.Core.Resources;
using Sharpnado.MaterialFrame;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleUnpaidView : MaterialFrame
    {
        public VehicleUnpaidView()
        {
            InitializeComponent();
            lblExpiredDate.Text = MobileResource.ListVehicle_Label_Vehicle_Expiration_Date;
        }
    }
}