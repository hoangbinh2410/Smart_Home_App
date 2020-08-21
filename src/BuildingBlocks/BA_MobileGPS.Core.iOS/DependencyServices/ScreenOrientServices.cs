
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
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("UISupportedInterfaceOrientations"));
            var a = UIDevice.CurrentDevice.GetDictionaryOfValuesFromKeys(new NSString[] { new NSString("UISupportedInterfaceOrientations") });
            foreach (var item in a)
            {
                var b = item.Key;
                var c = item.Value;
            }
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
            var d = UIDevice.CurrentDevice.GetDictionaryOfValuesFromKeys(new NSString[] { new NSString("orientation") });
            foreach (var item in d)
            {
                var b = item.Key;
                var c = item.Value;
            }
        }

        public void ForcePortrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("UISupportedInterfaceOrientations"));
            var a = UIDevice.CurrentDevice.GetDictionaryOfValuesFromKeys(new NSString[] { new NSString("UISupportedInterfaceOrientations") });
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
    }
}