using Rg.Plugins.Popup.Pages;

namespace BA_MobileGPS.Core.Views
{
    public partial class UpdateVersion : PopupPage
    {
        public UpdateVersion()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}