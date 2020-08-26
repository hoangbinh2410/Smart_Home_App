using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BA_MobileGPS.Core.iOS.CustomRenderer;
using BA_MobileGPS.Core.Views;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(DetailCamera), typeof(DetailCameraRenderer))]
namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class DetailCameraRenderer : PageRenderer
    {
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromNInt((int)(UIInterfaceOrientation.Portrait)), new NSString("orientation"));
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewController != null && ViewController.NavigationController != null)
                ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
        }
    }
}