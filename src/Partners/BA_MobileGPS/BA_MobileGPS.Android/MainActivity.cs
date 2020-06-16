using Android.App;
using Android.Content.PM;
using Android.OS;

using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Droid.Setup;

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

            Forms.SetFlags("FastRenderers_Experimental");
            Forms.SetFlags("CollectionView_Experimental");
            Forms.Init(this, bundle);

            ToolSetup.Initialize(this, bundle);

            LoadApplication(new BAGPSApp(new AndroidInitializer()));
        }
    }
}