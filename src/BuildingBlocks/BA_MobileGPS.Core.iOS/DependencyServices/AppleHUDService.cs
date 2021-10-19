using BA_MobileGPS.Core.iOS.DependencyServices;
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
                BTProgressHUD.BTProgressHUD.Show(null, -1, maskType: BTProgressHUD.MaskType.Black);
            }
            else
            {
                BTProgressHUD.BTProgressHUD.Show(message, -1, maskType: BTProgressHUD.MaskType.Black);
            }
        }

        public void Dismiss()
        {
            BTProgressHUD.BTProgressHUD.Dismiss();
        }
    }
}