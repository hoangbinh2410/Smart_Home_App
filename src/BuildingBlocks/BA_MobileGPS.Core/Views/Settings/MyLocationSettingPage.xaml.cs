using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class MyLocationSettingPage : ContentPage
    {
        public MyLocationSettingPage()
        {
            InitializeComponent();
            googleMap.IsTrafficEnabled = false;
            googleMap.IsUseCluster = false;
            //googleMap.MyLocationEnabled = true;

            googleMap.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), MobileUserSettingHelper.Mapzoom);

            googleMap.UiSettings.MapToolbarEnabled = false;
            googleMap.UiSettings.ZoomControlsEnabled = false;
            googleMap.UiSettings.MyLocationButtonEnabled = false;
            googleMap.UiSettings.RotateGesturesEnabled = false;
        }
    }
}