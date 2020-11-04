using Android.Content.PM;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenOrientServices))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class ScreenOrientServices : IScreenOrientServices
    {
        public void ForceLandscape()
        {
            var activity = CrossCurrentActivity.Current.Activity;
            activity.RequestedOrientation = ScreenOrientation.Landscape;
        }

        public void ForcePortrait()
        {
            var activity = CrossCurrentActivity.Current.Activity;
            activity.RequestedOrientation = ScreenOrientation.Portrait;
        }
    }
}