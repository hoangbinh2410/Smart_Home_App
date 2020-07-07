using BA_MobileGPS.Core.iOS.Factories;
using BA_MobileGPS.Utilities.Constant;

using FFImageLoading.Forms.Platform;

using Plugin.Toasts;
using Sharpnado.MaterialFrame.iOS;
using Sharpnado.Presentation.Forms.iOS;
using Shiny;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfDataGrid.XForms.iOS;
using Syncfusion.SfImageEditor.XForms.iOS;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.ComboBox;

using Xamarin.Forms.Platform.iOS;

namespace BA_MobileGPS.Core.iOS.Setup
{
    public static class ToolSetup
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

            SharpnadoInitializer.Initialize();

            // Override default ImageFactory by your implementation.
            FormsGoogleMaps.Init(Config.GoogleMapKeyiOS, new PlatformConfig
            {
                ImageFactory = new CachingImageFactory()
            });
            iOSShinyHost.Init(new ShinyAppStartup());

            iOSMaterialFrameRenderer.Init();

            // Syncfusion
            SfListViewRenderer.Init();
            //SfPickerRenderer.Init();
            SfDataGridRenderer.Init();
            SfCheckBoxRenderer.Init();
            SfSwitchRenderer.Init();
            SfComboBoxRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
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