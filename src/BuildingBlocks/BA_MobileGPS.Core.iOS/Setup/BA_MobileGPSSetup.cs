﻿using BA_MobileGPS.Core.iOS.Factories;
using BA_MobileGPS.Utilities.Constant;

using FFImageLoading.Forms.Platform;

using Plugin.Toasts;
using Shiny;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfDataGrid.XForms.iOS;
using Syncfusion.SfImageEditor.XForms.iOS;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.ComboBox;

using Xamarin.Forms.Platform.iOS;

namespace BA_MobileGPS.Core.iOS.Setup
{
    public static class BA_MobileGPS_iOS_Setup
    {
        public static FormsApplicationDelegate AppDelegate;

        public static void Initialize(FormsApplicationDelegate _AppDelegate)
        {
            AppDelegate = _AppDelegate;
            Xamarin.Forms.DependencyService.Register<ToastNotification>();
            ToastNotification.Init();

            Rg.Plugins.Popup.Popup.Init();

            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

            // Override default ImageFactory by your implementation.
            FormsGoogleMaps.Init(Config.GoogleMapKeyiOS, new PlatformConfig
            {
                ImageFactory = new CachingImageFactory()
            });
            iOSShinyHost.Init(new ShinyAppStartup());

            // Syncfusion
            SfListViewRenderer.Init();
            //SfPickerRenderer.Init();
            SfDataGridRenderer.Init();
            SfCheckBoxRenderer.Init();
            SfComboBoxRenderer.Init();
            //SfPullToRefreshRenderer.Init();
            SfImageEditorRenderer.Init();
            //SfCalendarRenderer.Init();
            //SfBadgeViewRenderer.Init();
            SfChartRenderer.Init();
            //SfBusyIndicatorRenderer.Init();
            //SfTabViewRenderer.Init();
            //SfRatingRenderer.Init();
            //SfPopupLayoutRenderer.Init();
            //SfMapsRenderer.Init();
            //SfRangeSliderRenderer.Init();
        }
    }
}