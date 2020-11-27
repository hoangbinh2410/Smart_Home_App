//using BA_MobileGPS.Core.Delegates.Shinny;
//using Microsoft.Extensions.DependencyInjection;
//using Prism.Unity;
//using Shiny;
//using Shiny.Prism;
//using static BA_MobileGPS.Core.Delegates.Shinny.GpsListener;

//namespace BA_MobileGPS.Core
//{
//    public class ShinyAppStartup : PrismStartup
//    {
//        public ShinyAppStartup() : base(PrismContainerExtension.Current)
//        {
//        }

//        protected override void ConfigureServices(IServiceCollection services)
//        {
//            services.AddSingleton<IGpsListener, GpsListener>();
//            services.UseGps<LocationDelegate>();
//        }
//    }
//}