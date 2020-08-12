using BA_MobileGPS.Core.iOS;
using BA_MobileGPS.Core.iOS.Setup;

using Foundation;

using UIKit;
using Xamarin.Forms;

namespace VMS_MobileGPS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : BaseDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags(new string[] { "CarouselView_Experimental", "IndicatorView_Experimental", "FastRenderers_Experimental", "AppTheme_Experimental", "RadioButton_Experimental" });
            Xamarin.Forms.Forms.Init();
            
            ToolSetup.Initialize(this);
            UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            UINavigationBar.Appearance.BackgroundColor = UIColor.Clear;
            UINavigationBar.Appearance.TintColor = UIColor.Clear;
            UINavigationBar.Appearance.BarTintColor = UIColor.Clear;
            UINavigationBar.Appearance.Translucent = false;
            UINavigationBar.Appearance.BarStyle = UIBarStyle.Black;

            RequestNotificationPermissions(app);

            LoadApplication(new VMSApp(new IOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }
}