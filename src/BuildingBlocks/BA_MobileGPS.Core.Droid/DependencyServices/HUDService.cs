using BA_MobileGPS.Core.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(HUDService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class HUDService : IHUDProvider
    {
        public void DisplayProgress(string message)
        {
            AndroidHUD.AndHUD.Shared.Show(Android.App.Application.Context, message);
        }

        public void Dismiss()
        {
            AndroidHUD.AndHUD.Shared.Dismiss(Android.App.Application.Context);
        }
    }
}