using BA_MobileGPS.Core.iOS.DependencyServices;
using Foundation;

using UIKit;

using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class SettingsService : ISettingsService
    {
        public bool OpenBluetoothSettings()
        {
            bool ok = false;
            try
            {
                var BluetoothURL = new NSUrl("App-Prefs:root=Bluetooth");

                if (UIApplication.SharedApplication.CanOpenUrl(BluetoothURL))
                {   //Pre iOS 10
                    UIApplication.SharedApplication.OpenUrl(BluetoothURL);
                }
                else
                {   //iOS 10
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
                }

                ok = true;
            }
            catch
            {
                ok = false;
            }
            return ok;
        }

        public bool OpenLocationSettings()
        {
            // Opening settings only open in iOS 8+
            if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                return false;

            try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool OpenWifiSettings()
        {
            bool ok = false;
            try
            {
                var WiFiURL = new NSUrl("prefs:root=WIFI");

                if (UIApplication.SharedApplication.CanOpenUrl(WiFiURL))
                {   //Pre iOS 10
                    UIApplication.SharedApplication.OpenUrl(WiFiURL);
                }
                else
                {   //iOS 10
                    UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=WIFI"));
                }

                ok = true;
            }
            catch
            {
                ok = false;
            }
            return ok;
        }
    }
}