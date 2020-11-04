using BA_MobileGPS.Core.Resources;
using Sharpnado.MaterialFrame;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleExpiredView : MaterialFrame
    {
        public VehicleExpiredView()
        {
            InitializeComponent();
            lblExpriedDate.Text = MobileResource.ListVehicle_Label_Vehicle_Expiration_Date;
        }
    }
}