using BA_MobileGPS.Core.Resources;
using Prism.Events;
using System;
using Xamarin.Forms.PancakeView;
using Xamarin.Forms.Xaml;
using Prism;
using Prism.Ioc;

namespace MOTO_MobileGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnlineCarInfoView : PancakeView
    {
        private readonly IEventAggregator eventAggregator;
        public OnlineCarInfoView()
        {
            InitializeComponent();
            lblVelocity.Text = MobileResource.Online_Label_SeachVehicle3.Trim().ToUpper();
            lblEngine.Text = MobileResource.Online_Label_Location.Trim().ToUpper();
            lblAirCondition.Text = MobileResource.Online_Label_StatusCarEngineOn.Trim().ToUpper();
            lblCarDoor.Text = MobileResource.Online_Label_StatusCarEngineOff.Trim().ToUpper();
            lblRoute.Text = MobileResource.Route_Label_Title.Trim().ToUpper();
            lblButtonDetail.Text = MobileResource.Online_Button_Detail.Trim().ToUpper();
            lblSetting.Text = MobileResource.Online_Button_Option.Trim().ToUpper();
            eventAggregator = PrismApplicationBase.Current.Container.Resolve<IEventAggregator>();
        }
        private void TapGetLocationVehicle_Tapped(object sender, EventArgs e)
        {
            eventAggregator.GetEvent<GetLocationVehicleEvent>().Publish();
        }


    }
    public class GetLocationVehicleEvent : PubSubEvent
    {

    }
}