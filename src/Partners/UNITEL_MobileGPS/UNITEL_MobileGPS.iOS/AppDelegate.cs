using BA_MobileGPS.Core.iOS;
using BA_MobileGPS.Core.iOS.Setup;
using BA_MobileGPS.Core.Views;
using Foundation;
using Prism.Common;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace UNITEL_MobileGPS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : BaseDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags(new string[] { "CarouselView_Experimental", "IndicatorView_Experimental", "FastRenderers_Experimental", "AppTheme_Experimental" });
            Xamarin.Forms.Forms.Init();

            ToolSetup.Initialize(this);

            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.Clear;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.BarStyle = UIBarStyle.Black;
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            RequestNotificationPermissions(app);

            LoadApplication(new UNITELApp(new IOSInitializer()));

            return base.FinishedLaunching(app, options);
        }

       
    }
}