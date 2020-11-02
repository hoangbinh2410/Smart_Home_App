using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.iOS.CustomRenderer;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class ExtendedTabbedPageRenderer : TabbedRenderer
    {
        public ExtendedTabbedPageRenderer()
        {
            
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
        }


        public UIImage ScalingImageToSize(UIImage sourceImage, CGSize newSize)
        {

            if (UIScreen.MainScreen.Scale == 2.0) //@2x iPhone 6 7 8 
            {
                UIGraphics.BeginImageContextWithOptions(newSize, false, 2.0f);
            }


            else if (UIScreen.MainScreen.Scale == 3.0) //@3x iPhone 6p 7p 8p...
            {
                UIGraphics.BeginImageContextWithOptions(newSize, false, 3.0f);
            }

            else
            {
                UIGraphics.BeginImageContext(newSize);
            }

            sourceImage.Draw(new CGRect(0, 0, newSize.Width, newSize.Height));

            UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return newImage;

        }
    }
}