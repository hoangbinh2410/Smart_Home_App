using Android.App;
using Android.Widget;
using BA_MobileGPS.Core.Droid.DependencyServices;
using Com.JeevanDeshmukh.GlideToastLib;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(DisplayMessageService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class DisplayMessageService : IDisplayMessage
    {
        public void ShowMessageError(string message, double time)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            new GlideToast.MakeToast(activity, message, (int)time, GlideToast.FailToast, GlideToast.Bottom).Show();
        }

        public void ShowMessageInfo(string message, double time)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            new GlideToast.MakeToast(activity, message, (int)time, GlideToast.InfoToast, GlideToast.Bottom).Show();
        }

        public void ShowMessageWarning(string message, double time)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            new GlideToast.MakeToast(activity, message, (int)time, GlideToast.WarningToast, GlideToast.Bottom).Show();
        }

        public void ShowMessageSuccess(string message, double time)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            new GlideToast.MakeToast(activity, message, (int)time, GlideToast.SuccessToast, GlideToast.Bottom).Show();
        }

        public void ShowToast(string message, double time)
        {
            Toast.MakeText(Forms.Context, message, ToastLength.Long).Show();
        }
    }
}