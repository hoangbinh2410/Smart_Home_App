
using BA_MobileGPS.Core.iOS.CustomRenderer;
using BA_MobileGPS.Core.Views;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(CameraStream), typeof(CameraStreamRenderer))]
namespace BA_MobileGPS.Core.iOS.CustomRenderer
{

    public class CameraStreamRenderer : PageRenderer
    {
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromNInt((int)(UIInterfaceOrientation.Portrait)), new NSString("orientation"));
        }
    }
}