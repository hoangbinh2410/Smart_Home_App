using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace BA_MobileGPS.Droid
{
    [Activity(Label = "@string/app_name", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var intent = new Intent(this, typeof(MainActivity));

            // Create your application here
            var mainIntent = new Intent(Application.Context, typeof(MainActivity));

            if (Intent.Extras != null)
            {
                mainIntent.PutExtras(Intent.Extras);
            }

            StartActivity(intent);
            Finish();
        }
    }
}