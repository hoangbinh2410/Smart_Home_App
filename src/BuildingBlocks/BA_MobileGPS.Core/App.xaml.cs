using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Core.Views;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities.Constant;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace BA_MobileGPS.Core
{
    public partial class App
    {
        /*
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor.
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetEnableAccessibilityScalingForNamedFontSizes(false);
            _eventAggregator = Current.Container.Resolve<IEventAggregator>();
        }

        private readonly IEventAggregator _eventAggregator;

        public static AppType AppType = AppType.BinhAnh;

        public virtual string OneSignalKey => Config.OneSignalKey;

        protected override IContainerExtension CreateContainerExtension() => PrismContainerExtension.Current;

        protected override void OnInitialized()
        {
            InitializeComponent();
            Resources.MergedDictionaries.Add(new Styles.Fonts());
            Resources.MergedDictionaries.Add(new Styles.Styles());
            Resources.MergedDictionaries.Add(new Styles.Converters());
            Resources.MergedDictionaries.Add(new Styles.Text());

            BA_MobileGPSSetup.Initialize();

            OneSignalHelper.RegisterOneSignal(OneSignalKey);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            BA_MobileGPSSetup.RegisterServices(containerRegistry);
            BA_MobileGPSSetup.RegisterPages(containerRegistry);            
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _eventAggregator.GetEvent<OnResumeEvent>().Publish(true);
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            _eventAggregator.GetEvent<OnSleepEvent>().Publish(true);
        }
    }
}