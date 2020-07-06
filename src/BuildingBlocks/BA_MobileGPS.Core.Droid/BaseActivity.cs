using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Utilities.Enums;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Rg.Plugins.Popup.Services;
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
                containerRegistry.RegisterInstance<IDisplayMessage>(new DisplayMessageService());
                //containerRegistry.RegisterInstance<ISettingsService>(new SettingsService());
                //containerRegistry.RegisterInstance<ISaveAndView>(new SaveAndViewAndroid());
                //containerRegistry.RegisterInstance<IAudioManager>(new DroidAudioManager());
                containerRegistry.RegisterInstance<ITooltipService>(new DroidTooltipService());
                //containerRegistry.RegisterInstance<IDownloader>(new AndroidDownloader());
                containerRegistry.RegisterInstance<IAppVersionService>(new AppVersionService());
            }
        }

        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                await PopupNavigation.Instance.PopAsync();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();          
        }

        protected override void OnResume()
        {
            base.OnResume();          
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
           
        }

      
    }
}