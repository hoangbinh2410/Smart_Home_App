using BA_MobileGPS.Core;
using BigTed;
using Xamarin.Forms;

[assembly: Dependency(typeof(HUDService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class HUDService : IHUDProvider
    {
        public void DisplayProgress(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                BTProgressHUD.Show(null, -1, ProgressHUD.MaskType.Black);
            }
            else
            {
                BTProgressHUD.Show(message, -1, ProgressHUD.MaskType.Black);
            }
        }

        public void Dismiss()
        {
            BTProgressHUD.Dismiss();
        }
    }
}