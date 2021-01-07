using Android.App;
using Android.OS;
using Android.Runtime;

using Plugin.CurrentActivity;
using Shiny;
using System;

namespace VMS_MobileGPS.Droid
{
#if DEBUG

    [Application(Debuggable = true)]
#else
[Application(Debuggable = false)]
#endif
    [MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyDwhz_8SoIcFYMLVh3rcto1cWGbAPdQfGI")]
    public class MainApplication : ShinyAndroidApplication<ShinyAppStartup>, Android.App.Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);
        }
    }
}