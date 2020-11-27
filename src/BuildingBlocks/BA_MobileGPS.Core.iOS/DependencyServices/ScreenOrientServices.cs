using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.iOS.DependencyServices;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenOrientServices))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class ScreenOrientServices : IScreenOrientServices
    {
        public void ForceLandscape()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
            UIViewController.AttemptRotationToDeviceOrientation();
        }

        public void ForcePortrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
            UIViewController.AttemptRotationToDeviceOrientation();
        }
    }
}