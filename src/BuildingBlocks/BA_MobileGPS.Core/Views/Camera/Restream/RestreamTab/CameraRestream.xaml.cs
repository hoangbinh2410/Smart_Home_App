using BA_MobileGPS.Core.Controls;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BA_MobileGPS.Core.Views
{
    public partial class CameraRestream : TabbedPageEx
    {
        private double _width;
        private double _height;
        private Xamarin.Forms.Page currentChildPage;
        private bool firstLoad { get; set; }

        public CameraRestream()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSmoothScrollEnabled(false);
            }
            else
            {
                On<iOS>().SetUseSafeArea(true);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            firstLoad = true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            using (new HUDService())
            {
                var oldWidth = _width;
                const double sizenotallocated = -1;

                base.OnSizeAllocated(width, height);
                if (Equals(_width, width) && Equals(_height, height)) return;

                _width = width;
                _height = height;

                // ignore if the previous height was size unallocated
                if (Equals(oldWidth, sizenotallocated)) return;

                // Has the device been rotated ?
                if (!Equals(width, oldWidth))
                {
                    if (firstLoad)
                    {
                        if (width < height)
                        {
                            OrientChangedToVetical();
                        }
                        else
                        {
                            OrientChangedToLanscape();
                        }
                    }
                }
            }
        }

        private void OrientChangedToLanscape()
        {
            var param = new NavigationParameters()
            {
                { "FullScreen", true }
            };
            PageUtilities.OnNavigatedTo(CurrentPage, param);
            Device.BeginInvokeOnMainThread(() =>
            {
                IsHidden = true;
            });
        }

        private void OrientChangedToVetical()
        {
            var param = new NavigationParameters()
            {
                { "FullScreen", false }
            };
            PageUtilities.OnNavigatedTo(CurrentPage, param);
            Device.BeginInvokeOnMainThread(() =>
            {
                IsHidden = false;
            });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var parameters = new NavigationParameters();
            var newPage = (ContentPage)CurrentPage;
            if (currentChildPage != null)
            {
                PageUtilities.OnNavigatedFrom(currentChildPage, parameters);
            }
            PageUtilities.OnNavigatedTo(newPage, parameters);
            currentChildPage = newPage;
        }
    }
}