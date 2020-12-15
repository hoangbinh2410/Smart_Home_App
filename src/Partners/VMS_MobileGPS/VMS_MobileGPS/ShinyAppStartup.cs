using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using Shiny;
using Shiny.Prism;
using VMS_MobileGPS.Delegates.Shinny;
using static VMS_MobileGPS.Delegates.Shinny.GpsListener;

namespace VMS_MobileGPS
{
    public class ShinyAppStartup : PrismStartup
    {
        public ShinyAppStartup() : base(PrismContainerExtension.Current)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGpsListener, GpsListener>();
            services.UseGps<LocationDelegate>();
        }
    }
}
