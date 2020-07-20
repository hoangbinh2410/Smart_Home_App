using Plugin.GoogleAnalytics;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Helpers
{
    public static class GoogleAnalyticsHelper
    {
        public static void RegisterGoogleAnalyticsHelper()
        {
            GoogleAnalytics.Current.Config.TrackingId = "UA-240301113";
            GoogleAnalytics.Current.Config.AppId = "AppID";
            GoogleAnalytics.Current.Config.AppName = "BASAT";
            GoogleAnalytics.Current.Config.AppVersion = DependencyService.Get<IAppVersionService>().GetAppVersion();
            GoogleAnalytics.Current.InitTracker();
        }
    }
}