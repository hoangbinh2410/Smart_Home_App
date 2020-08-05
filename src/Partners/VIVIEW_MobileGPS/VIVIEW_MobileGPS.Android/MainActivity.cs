using Android.App;
using Android.Content.PM;
using Android.OS;
using VIVIEW_MobileGPS;
using BA_MobileGPS.Core.Droid;
using BA_MobileGPS.Droid.Setup;
using Xamarin.Forms;

namespace VIVIEW_MobileGPS.Droid
{
    [Activity(Label = "VIVIEW GPS 2", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false,
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

            LoadApplication(new VIVIEWApp(new AndroidInitializer()));
        }
    }
}