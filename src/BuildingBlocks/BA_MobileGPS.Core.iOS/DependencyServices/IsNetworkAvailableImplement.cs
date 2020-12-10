using BA_MobileGPS.Core.iOS.DependencyServices;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(IsNetworkAvailableImplement))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class IsNetworkAvailableImplement : INetworkAvailable
    {
        public IsNetworkAvailableImplement()
        {
        }

        bool INetworkAvailable.IsNetworkAvailable()
        {
            NSString urlString = new NSString("https://captive.apple.com");

            NSUrl url = new NSUrl(urlString);

            NSUrlRequest request = new NSUrlRequest(url, NSUrlRequestCachePolicy.ReloadIgnoringCacheData, 3);

            NSData data = NSUrlConnection.SendSynchronousRequest(request, out NSUrlResponse response, out NSError error);

            NSString result = NSString.FromData(data, NSStringEncoding.UTF8);

            if (result.Contains(new NSString("Success")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}