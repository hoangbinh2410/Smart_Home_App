using Android.App;
using Android.OS;

using BA_MobileGPS.Core.Droid;
using FFImageLoading.Forms.Platform;
using PanCardView.Droid;
using Plugin.Toasts;
using Sharpnado.Presentation.Forms.Droid;

namespace BA_MobileGPS.Droid.Setup
{
    public static class ToolSetup
    {
        public static void Initialize(Activity activity, Bundle bundle)
        {
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(activity, bundle);

            Xamarin.Forms.DependencyService.Register<ToastNotification>();

            ToastNotification.Init(activity, new PlatformOptions() { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

            Xamarin.Essentials.Platform.Init(activity, bundle); // add this line to your code, it may also be called: bundle

            Rg.Plugins.Popup.Popup.Init(activity, bundle);

            SharpnadoInitializer.Initialize();

            CardsViewRenderer.Preserve();

            CachedImageRenderer.Init(enableFastRenderer: true);
            CachedImageRenderer.InitImageViewHandler();
            //This forces the custom renderers to be used

            // Override default BitmapDescriptorFactory by your implementation
            FormsGoogleMaps.Init(activity, bundle, new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            });
        }
    }
}