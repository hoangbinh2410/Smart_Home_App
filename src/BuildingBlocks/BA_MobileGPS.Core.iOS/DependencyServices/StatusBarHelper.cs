using BA_MobileGPS.Core.iOS.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBarHelper))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class StatusBarHelper : IStatusBar
    {
        public void ChangeStatusBarColorToWhite()
        {
            UIKit.UIApplication.SharedApplication.StatusBarStyle = UIKit.UIStatusBarStyle.LightContent;
        }

        public void ChangeStatusBarColorToBlack()
        {
            UIKit.UIApplication.SharedApplication.StatusBarStyle = UIKit.UIStatusBarStyle.Default;
        }

        public void HideStatusBar()
        {
            UIKit.UIApplication.SharedApplication.StatusBarStyle = UIKit.UIStatusBarStyle.BlackTranslucent;
        }
    }
}