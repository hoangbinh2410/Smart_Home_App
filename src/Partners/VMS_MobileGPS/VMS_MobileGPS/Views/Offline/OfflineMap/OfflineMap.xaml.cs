using Syncfusion.SfMaps.XForms;
using VMS_MobileGPS.Constant;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace VMS_MobileGPS.Views
{
    public partial class OfflineMap : ContentPage
    {
        public OfflineMap()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                SfMap.EnableZooming = false;
            }
        }

        private void ImageryLayer_ZoomLevelChanging(object sender, ZoomLevelChangingEventArgs e)
        {
            if (e.CurrentLevel > SfMap.MaxZoom || e.CurrentLevel < SfMap.MinZoom)
            {
                e.Cancel = true;
            }
            else
                GlobalResourcesVMS.Current.OffMapZoomLevel = e.CurrentLevel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.RuntimePlatform == Device.iOS)
            {
                var safe = On<iOS>().SafeAreaInsets();
                Padding = new Thickness(0, 0, 0, safe.Bottom);
            }
        }
    }
}