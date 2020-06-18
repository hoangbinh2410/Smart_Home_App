﻿using BA_MobileGPS.Core;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Constant;
using BA_MobileGPS.Utilities.Enums;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;

using VMS_MobileGPS.Styles;

namespace VMS_MobileGPS
{
    public class VMSApp : App
    {
        public VMSApp(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public override string OneSignalKey => Config.OneSignalKey_BASAT;

        protected async override void OnInitialized()
        {
            Resources.MergedDictionaries.Add(new Colors());

            base.OnInitialized();

            ServerConfig.ServerTypes = ServerTypes.ServerVMS;
            ServerConfig.ApiEndpointTypes = ApiEndpointTypes.ServerVMS;

            AppCenter.Start("ios=9a0650ec-057e-4e5a-b8de-4c3fd1fae415;" +
                  "android=28d78b27-4b62-42e5-8db5-8e2d50de6a3a",
                  typeof(Analytics), typeof(Crashes));

            //AppManager.Init();

            //await NavigationService.NavigateAsync("/NavigationPage/LandingPage");
            await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);

            AppType = BA_MobileGPS.Entities.AppType.VMS;

            // Đăng ký config automapper
            AutoMapperConfig.RegisterMappings(containerRegistry);

            containerRegistry.Register<IMessageService, MessageService>();
            containerRegistry.Register<IFishShipService, FishShipService>();
            containerRegistry.Register<ISOSHistoryService, SOSHistoryService>();
            containerRegistry.Register<IVehicleDebtBlockService, VehicleDebtBlockService>();

            containerRegistry.Register<IServicePackageHistoryService, ServicePackageHistoryService>();
            
        }
    }
}