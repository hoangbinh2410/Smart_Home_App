using Prism.Navigation;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Views
{
    public partial class DeviceTab : ContentPage,INavigationAware
    {
        private readonly double portraitHeight = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height / 
                                                 Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density;
        private readonly double videoHeightRatio = 2.7;
        public DeviceTab()
        {
            InitializeComponent();           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainTopLayout.HeightRequest = portraitHeight/ videoHeightRatio;
            
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

    }
}
