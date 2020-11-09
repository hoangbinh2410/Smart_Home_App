using Android.Content.PM;
using BA_MobileGPS.Core.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(AppVersionService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class AppVersionService : IAppVersionService
    {
        public string GetAppVersion()
        {
            var context = Android.App.Application.Context;

            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionName;
        }

        public string GetAppBuild()
        {
            var context = Android.App.Application.Context;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);

            return info.VersionCode.ToString();
        }
    }
}