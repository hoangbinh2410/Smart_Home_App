using BA_MobileGPS.Core.Resources;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlineCarInfoView : PancakeView
    {
        public OnlineCarInfoView()
        {
            InitializeComponent();
            lblVelocity.Text = MobileResource.Online_Label_SeachVehicle3.Trim().ToUpper();
            lblEngine.Text = MobileResource.Online_Label_Location.Trim().ToUpper();
            lblAirCondition.Text = MobileResource.Online_Label_StatusCarEngineOn.Trim().ToUpper();
            lblCarDoor.Text = MobileResource.Online_Label_StatusCarEngineOff.Trim().ToUpper();
            lblRoute.Text = MobileResource.Route_Label_Title.Trim().ToUpper();
            lblButtonDetail.Text = MobileResource.Online_Button_Detail.Trim().ToUpper();
        }
        
    }
}