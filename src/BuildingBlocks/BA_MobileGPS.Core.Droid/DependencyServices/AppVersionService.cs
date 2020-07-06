using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;

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