using Android.App;
using Android.OS;

using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Core.Droid.DependencyServices;
using FFImageLoading.Forms.Platform;
using LibVLCSharp.Forms.Shared;
using PanCardView.Droid;
using Sharpnado.Presentation.Forms.Droid;
using Syncfusion.XForms.Android.PopupLayout;

namespace BA_MobileGPS.Droid.Setup
{
    public static class ToolSetup
    {
        public static void Initialize(Activity activity, Bundle bundle)
        {
            SfPopupLayoutRenderer.Init();

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(activity, bundle);

            Xamarin.Essentials.Platform.Init(activity, bundle); // add this line to your code, it may also be called: bundle

            Rg.Plugins.Popup.Popup.Init(activity);

            SharpnadoInitializer.Initialize();

            CardsViewRenderer.Preserve();

            CachedImageRenderer.Init(enableFastRenderer: true);
            CachedImageRenderer.InitImageViewHandler();
            //This forces the custom renderers to be used
            Android.Glide.Forms.Init(activity, debug: false);

            // Override default BitmapDescriptorFactory by your implementation
            FormsGoogleMaps.Init(activity, bundle, new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            });

            PlatformImageViewer.Init(activity);
            LibVLCSharpFormsRenderer.Init();

            Stormlion.PhotoBrowser.Droid.Platform.Init(activity);
        }
    }
}