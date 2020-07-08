using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

[assembly: Dependency(typeof(BA_MobileGPS.Core.Droid.Application))]

namespace BA_MobileGPS.Core.Droid
{
    public class Application : IApplication
    {
        public void SetWindowSoftInputMode(WindowSoftInputModeAdjust inputMode)
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(inputMode);
        }
    }
}