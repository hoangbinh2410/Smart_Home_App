using BA_MobileGPS.Core.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace BA_MobileGPS.Core.Views
{
    public partial class UploadVideoProssessPage : PopupPage
    {
        public UploadVideoProssessPage()
        {
            InitializeComponent();
        }

        private void PopupPage_BackgroundClicked(object sender, System.EventArgs e)
        {
            ((UploadVideoProssessPageViewModel)BindingContext).Close();
        }
    }
}