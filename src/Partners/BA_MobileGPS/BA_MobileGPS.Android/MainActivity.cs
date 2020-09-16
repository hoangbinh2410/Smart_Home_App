using Android.App;
using Android.Content.PM;
using Android.OS;

using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Droid.Setup;
using System.Diagnostics;
using Xamarin.Forms;

namespace BA_MobileGPS.Droid
{
    [Activity(Label = "BA_MobileGPS", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false,
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

            Stopwatch sw = new Stopwatch();
            sw.Start();
            ToolSetup.Initialize(this, bundle);
            sw.Stop();
            System.Diagnostics.Debug.WriteLine(string.Format("ToolSetup.Initialize : {0}", sw.ElapsedMilliseconds));

            LoadApplication(new BAGPSApp(new AndroidInitializer()));
        }
    }
}