using BA_MobileGPS.Core.ViewModels;
using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class ViewVideoUploadedPage : ContentPage, INavigationAware
    {
        private readonly double portraitHeight = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height /
                                                 Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;

        private readonly double videoHeightRatio = 3.2;
        private ViewVideoUploadedPageViewModel vm;

        public ViewVideoUploadedPage()
        {
            InitializeComponent();
            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(MobileUserSettingHelper.LatCurrentScreenMap, MobileUserSettingHelper.LngCurrentScreenMap), 18d);
            map.IsUseCluster = false;
            map.IsTrafficEnabled = false;
            map.UiSettings.ZoomGesturesEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;
            map.UiSettings.RotateGesturesEnabled = false;
            vm = (ViewVideoUploadedPageViewModel)BindingContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainTopLayout.HeightRequest = portraitHeight / videoHeightRatio;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("FullScreen") && parameters.GetValue<bool>("FullScreen") is bool fullScreen)
            {
                if (fullScreen)
                {
                    OrientChangedToLanscape();
                }
                else OrientChangedToVetical();
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        private void OrientChangedToLanscape()
        {
            mainTopLayout.HeightRequest = portraitHeight;
        }

        private void OrientChangedToVetical()
        {
            mainTopLayout.HeightRequest = portraitHeight / videoHeightRatio;
        }

        private void SeekBar_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            vm.SeekBarValueChanged(e.NewValue);
        }
    }
}
