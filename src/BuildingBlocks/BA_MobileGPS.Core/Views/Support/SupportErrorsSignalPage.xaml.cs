using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class SupportErrorsSignalPage : ContentPage
    {
        public SupportErrorsSignalPage()
        {
            InitializeComponent();
            //this.title.Text = MobileResource.SupportClient_Label_Title;
            this.lostSignal.Text = MobileResource.SupportClient_Label_LostSignal;
            this.textSupportCameraError.Text = MobileResource.SupportClient_Label_TextSupportCameraError;
            this.SfButtonClose.Text = MobileResource.SupportClient_Button_Close;
        }
    }
}
