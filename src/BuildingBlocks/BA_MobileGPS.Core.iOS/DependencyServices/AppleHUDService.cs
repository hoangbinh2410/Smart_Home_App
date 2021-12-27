using BA_MobileGPS.Core.iOS.DependencyServices;
using BigTed;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppleHUDService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class AppleHUDService : IHUDProvider
    {
        public void DisplayProgress(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                BTProgressHUD.Show(null, -1, MaskType.Black);
            }
            else
            {
                BTProgressHUD.Show(message, -1, MaskType.Black);
            }
        }

        public void Dismiss()
        {
            BTProgressHUD.Dismiss();
        }
    }
}