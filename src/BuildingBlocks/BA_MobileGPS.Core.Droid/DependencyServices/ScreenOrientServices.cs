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