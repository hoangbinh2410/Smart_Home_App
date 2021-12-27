using BA_MobileGPS.Core.Resources;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlineCarInfoView : PancakeView
    {
        public OnlineCarInfoView()
        {
            InitializeComponent();
            lblVelocity.Text = MobileResource.Online_Label_Velocity.Trim().ToUpper();
            lblEngine.Text = MobileResource.Online_Label_Engine.Trim().ToUpper();
            lblAirCondition.Text = MobileResource.Online_Label_AirConditioning.Trim().ToUpper();
            lblCarDoor.Text = MobileResource.Online_Label_Cardoor.Trim().ToUpper();
            lblRoute.Text = MobileResource.Route_Label_Title.Trim().ToUpper();
            lblButtonDetail.Text = MobileResource.Online_Button_Detail.Trim().ToUpper();
            viewmore.Text=MobileResource.Online_Lable_ViewMore;
        }
    }
}