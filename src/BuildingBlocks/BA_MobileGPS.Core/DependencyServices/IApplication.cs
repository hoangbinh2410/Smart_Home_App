using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace BA_MobileGPS.Core
{
    public interface IApplication
    {
        void SetWindowSoftInputMode(WindowSoftInputModeAdjust inputMode);
    }
}