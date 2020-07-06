using BA_MobileGPS.Core.iOS.DependencyServices;

using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(AppVersionService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class AppVersionService : IAppVersionService
    {
        public string GetAppVersion()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString();
        }

        public string GetAppBuild()

        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString();
        }
    }
}