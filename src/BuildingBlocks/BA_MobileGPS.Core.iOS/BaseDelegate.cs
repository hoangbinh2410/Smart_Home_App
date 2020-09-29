using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.iOS.DependencyServices;
using BA_MobileGPS.Core.Views;
using Foundation;
using Prism;
using Prism.Common;
using Prism.Ioc;
using System.Linq;
using UIKit;

using UserNotifications;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.iOS
{
    public class BaseDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        protected void RequestNotificationPermissions(UIApplication app)
        {
            // Request Permissions
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request Permissions
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
                {
                    // Do something if needed
                });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                app.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null));
            }
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
                return true;

            return base.OpenUrl(app, url, options);
        }       

        protected class IOSInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                // Register any platform specific implementations
                containerRegistry.RegisterInstance<IDisplayMessage>(new DisplayMessageService());
                containerRegistry.RegisterInstance<ISettingsService>(new SettingsService());
                containerRegistry.RegisterInstance<IAppVersionService>(new AppVersionService());
                containerRegistry.RegisterInstance<IHUDProvider>(new BA_MobileGPS.Core.iOS.DependencyServices.AppleHUDService());
                containerRegistry.RegisterInstance<ISaveService>(new SaveService());
                //containerRegistry.RegisterInstance<IAccountKitService>(new AccountKitService());
                //containerRegistry.RegisterInstance<ISaveAndView>(new SaveAndViewIOS());
                containerRegistry.RegisterInstance<IAudioManager>(new AppleAudioManager());
                containerRegistry.RegisterInstance<ITooltipService>(new iOSTooltipService());
                containerRegistry.RegisterInstance<IDownloader>(new IosDownloader());
                containerRegistry.RegisterInstance<IScreenOrientServices>(new ScreenOrientServices());
            }
        }
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            var mainPage = Xamarin.Forms.Application.Current.MainPage;
            if (PageUtilities.GetCurrentPage(mainPage) is CameraManagingPage
                && UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                return UIInterfaceOrientationMask.AllButUpsideDown;
            }
            return UIInterfaceOrientationMask.Portrait;
        }
    }
}