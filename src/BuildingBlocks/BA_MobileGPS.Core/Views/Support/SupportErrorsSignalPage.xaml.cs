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
            this.textSupportError.Text = MobileResource.SupportClient_Label_textSupportError;
            this.SfButtonClose.Text = MobileResource.SupportClient_Button_Close;
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            
        }
    }
}
