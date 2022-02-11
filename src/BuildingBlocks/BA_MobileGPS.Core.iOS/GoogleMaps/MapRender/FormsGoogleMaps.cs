using Google.Maps;

namespace BA_MobileGPS.Core.iOS
{
    public static class FormsGoogleMaps
    {
        public static bool IsInitialized { get; private set; }

        public static void Init(string apiKey, PlatformConfig config = null)
        {
            MapServices.ProvideApiKey(apiKey);
            MapRenderer.Config = config ?? new PlatformConfig();
            IsInitialized = true;
        }
    }
}