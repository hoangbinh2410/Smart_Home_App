using BA_MobileGPS.Core.Resources;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class FeedbackErrorsSignalPage : ContentPage
    {
        public FeedbackErrorsSignalPage()
        {
            InitializeComponent();
            this.title.Text = MobileResource.SupportClient_Label_Title;
            this.textSupportFeedbackCameraError.Text = MobileResource.SupportClient_Label_TextSupportFeedbackCameraError;
            this.lbSupportFeedbackName.Text = MobileResource.SupportClient_Label_LbSupportFeedbackName;
            this.lbVehicle.Text = MobileResource.SupportClient_Label_LbSupportFeedbackVehicle;
            this.lbSupportFeedbackPhoneNumber.Text = MobileResource.SupportClient_Label_LbSupportFeedbackPhoneNumber;
            this.entrySupportFeedbackPhoneNumber.Placeholder = MobileResource.SupportClient_Label_LbSupportFeedbackPhoneNumber;
            this.lbSupportFeedbackContent.Text = MobileResource.SupportClient_Label_TextSupportFeedbackContent;
            this.editorSupportFeedbackContent.Placeholder = MobileResource.SupportClient_Label_TextSupportFeedbackContent;
            this.SfBtnSend.Text = MobileResource.SupportClient_Button_BtnSupportFeedbackSend;

        }
    }
}
