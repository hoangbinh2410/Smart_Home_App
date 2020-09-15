﻿using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Runtime;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using BA_MobileGPS.Core.Views;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Shiny;
using System.Diagnostics;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Droid
{
    public class BaseActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
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
                Stopwatch sw = new Stopwatch();
                sw.Start();
                // Register any platform specific implementations
                containerRegistry.RegisterInstance<IDisplayMessage>(new DisplayMessageService());
                containerRegistry.RegisterInstance<ISettingsService>(new SettingsService());
                //containerRegistry.RegisterInstance<ISaveAndView>(new SaveAndViewAndroid());
                containerRegistry.RegisterInstance<ISaveService>(new SaveService());
                containerRegistry.RegisterInstance<IAudioManager>(new DroidAudioManager());
                containerRegistry.RegisterInstance<ITooltipService>(new DroidTooltipService());
                containerRegistry.RegisterInstance<IDownloader>(new AndroidDownloader());
                containerRegistry.RegisterInstance<IAppVersionService>(new AppVersionService());
                containerRegistry.RegisterInstance<IScreenOrientServices>(new ScreenOrientServices());
                sw.Stop();
                Debug.WriteLine(string.Format("RegisterTypes : {0}", sw.ElapsedMilliseconds));
            }
        }

        
     
    }
}