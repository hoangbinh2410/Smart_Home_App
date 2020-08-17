using BA_MobileGPS.Core.Droid.DependencyServices;

using Com.Stfalcon.Frescoimageviewer;

using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(PhotoBrowserImplementation))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class PhotoBrowserImplementation : IPhotoBrowser
    {
        protected static ImageViewer _imageViewer;

        public void Show(PhotoBrowser photoBrowser)
        {
            ImageViewer.Builder builder = new ImageViewer.Builder(PlatformImageViewer.Context, photoBrowser.Photos.Select(x => x.URL).ToArray());
            ImageOverlayView overlay = new ImageOverlayView(PlatformImageViewer.Context, photoBrowser);

            builder.SetBackgroundColor(ColorExtensions.ToAndroid(photoBrowser.BackgroundColor));

            builder.SetOverlayView(overlay);
            builder.SetContainerPaddingPx(photoBrowser.Android_ContainerPaddingPx);

            builder.SetImageChangeListener(overlay);
            builder.SetStartPosition(photoBrowser.StartIndex);
            _imageViewer = builder.Show();
        }

        public void Close()
        {
            if (_imageViewer != null)
            {
                _imageViewer.OnDismiss();
                _imageViewer = null;
            }
        }
    }
}