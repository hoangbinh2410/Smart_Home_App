using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Droid.Setup;
using Xamarin.Forms;

namespace GISVIET_MobileGPS.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false,
      LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            base.OnCreate(bundle);
            this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            Forms.SetFlags(new string[] { "CarouselView_Experimental", "IndicatorView_Experimental", "FastRenderers_Experimental", "AppTheme_Experimental" });

            Forms.Init(this, bundle);
            Syncfusion.XForms.Android.Core.Core.Init(this);
            ToolSetup.Initialize(this, bundle);

            LoadApplication(new GISVIETApp(new AndroidInitializer()));
        }
    }
}

