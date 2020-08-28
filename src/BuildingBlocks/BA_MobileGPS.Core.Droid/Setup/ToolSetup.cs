using Android.App;
using Android.OS;

using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Core.Droid.DependencyServices;
using FFImageLoading.Forms.Platform;
using LabelHtml.Forms.Plugin.Droid;
using LibVLCSharp.Forms.Platforms.Android;
using LibVLCSharp.Forms.Shared;
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
            Android.Glide.Forms.Init(activity, debug: false);

            // Override default BitmapDescriptorFactory by your implementation
            FormsGoogleMaps.Init(activity, bundle, new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            });

            //Html Label
            HtmlLabelRenderer.Initialize();

            PlatformImageViewer.Init(activity);
            LibVLCSharpFormsRenderer.Init();
        }
    }
}