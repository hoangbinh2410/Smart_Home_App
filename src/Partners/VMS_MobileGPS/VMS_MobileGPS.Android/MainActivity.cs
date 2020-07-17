using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Droid.Setup;
using Xamarin.Forms;

namespace VMS_MobileGPS.Droid
{
    [Activity(Label = "BA-SAT", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false,
      LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Forms.SetFlags(new string[] { "CarouselView_Experimental", "IndicatorView_Experimental", "FastRenderers_Experimental", "AppTheme_Experimental" });

            Forms.Init(this, bundle);

            ToolSetup.Initialize(this, bundle);

            LoadApplication(new VMSApp(new AndroidInitializer()));
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
            }
        }
    }
}