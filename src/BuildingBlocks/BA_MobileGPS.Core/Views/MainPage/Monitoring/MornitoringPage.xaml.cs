using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MornitoringPage : ScrollView
    {
        public MornitoringPage()
        {
            InitializeComponent();
            googleMap.UiSettings.ZoomControlsEnabled = false;

            double scaleheight = (App.Current.MainPage.Width / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Width) * Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;
            popmenu.PopupView.StartY = (int)(scaleheight - popmenu.HeightRequest) + 50;
        }
    }
}
