using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Utilities.Enums;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Shiny;
using System;

namespace BA_MobileGPS.Core.Droid
{
    public class BaseActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            // Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void AttachBaseContext(Context @base)
        {
            var config = new Configuration(@base.Resources.Configuration)
            {
                FontScale = 1f
            };

            base.AttachBaseContext(@base.CreateConfigurationContext(config));
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                // Register any platform specific implementations
                //containerRegistry.RegisterInstance<IDisplayMessage>(new DisplayMessageService());
                //containerRegistry.RegisterInstance<ISettingsService>(new SettingsService());
                //containerRegistry.RegisterInstance<IAppVersionService>(new AppVersionService());
                //containerRegistry.RegisterInstance<IAccountKitService>(new AccountKitService());
                //containerRegistry.RegisterInstance<ISaveAndView>(new SaveAndViewAndroid());
                //containerRegistry.RegisterInstance<IAudioManager>(new DroidAudioManager());
                //containerRegistry.RegisterInstance<ITooltipService>(new DroidTooltipService());
                //containerRegistry.RegisterInstance<IDownloader>(new AndroidDownloader());
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            UpdateTheme(Resources.Configuration);
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateTheme(Resources.Configuration);
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            UpdateTheme(newConfig);
        }

        private void UpdateTheme(Configuration newConfig)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var themeService = Prism.PrismApplicationBase.Current.Container.Resolve<IThemeService>();
                var uiModeFlags = newConfig.UiMode & UiMode.NightMask;
                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        themeService.UpdateTheme(ThemeMode.Dark);
                        break;

                    case UiMode.NightNo:
                        themeService.UpdateTheme(ThemeMode.Light);
                        break;

                    default:
                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
                }
            }
        }
    }
}