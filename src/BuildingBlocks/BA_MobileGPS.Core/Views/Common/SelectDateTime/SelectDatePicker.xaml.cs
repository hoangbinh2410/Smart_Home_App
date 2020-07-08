using Rg.Plugins.Popup.Pages;

namespace BA_MobileGPS.Core.Views
{
    public partial class SelectDatePicker : PopupPage
    {
        public SelectDatePicker()
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