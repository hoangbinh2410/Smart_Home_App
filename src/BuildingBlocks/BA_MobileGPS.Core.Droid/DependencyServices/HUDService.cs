using BA_MobileGPS.Core.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(HUDService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class HUDService : IHUDProvider
    {
        public void DisplayProgress(string message)
        {
            AndroidHUD.AndHUD.Shared.Show(Forms.Context, message);
        }

        public void Dismiss()
        {
            AndroidHUD.AndHUD.Shared.Dismiss(Forms.Context);
        }
    }
}