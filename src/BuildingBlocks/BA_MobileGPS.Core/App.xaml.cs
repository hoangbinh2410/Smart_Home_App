using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Styles;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities.Constant;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("fa-regular-400.ttf", Alias = "FontAwesomeRegular")]
[assembly: ExportFont("fa-solid-900.ttf", Alias = "FontAwesomeSolid")]
[assembly: ExportFont("fa-brands-400.ttf", Alias = "FontAwesomeBrands")]

namespace BA_MobileGPS.Core
{
    public partial class App
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
            _eventAggregator = Current.Container.Resolve<IEventAggregator>();
        }



        private readonly IEventAggregator _eventAggregator;

        public static AppType AppType = AppType.BinhAnh;

        public static string CurrentLanguage = Settings.CurrentLanguage;

        public virtual string OneSignalKey => Config.OneSignalKey;

        protected override void OnInitialized()
        {
            InitializeComponent();

            BA_MobileGPSSetup.Initialize();

            OneSignalHelper.RegisterOneSignal(OneSignalKey);

            SetTheme();
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

        private void SetTheme()
        {
            Resources.MergedDictionaries.Add(new Styles.Converters());
            Resources.MergedDictionaries.Add(new Fonts());
        }
    }
}