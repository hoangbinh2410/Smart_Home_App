using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BA_MobileGPS.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupportClientPage : ContentPage
    {
        public SupportClientPage()
        {
            InitializeComponent();
            this.title.Text = MobileResource.SupportClient_Label_Title;
            this.vehicleProcessing.Text = MobileResource.SupportClient_Label_VehicleProcessing;
            this.supportCategory.Text = MobileResource.SupportClient_Label_SupportCategory;
            this.textSupport.Text = MobileResource.SupportClient_Label_TextSupport;
            this.entrySearch.Placeholder = "Chọn phương tiện";
            //this.lostSignal.Text = MobileResource.SupportClient_Label_LostSignal;
            //this.changeNumberPlate.Text = MobileResource.SupportClient_Label_ChangeNumberPlate;
            //this.cameraError.Text = MobileResource.SupportClient_Label_CameraError;
        }
    }
}