using System.Net;

namespace BA_MobileGPS.Core.Helpers
{
    public static class NetworkHelper
    {
        public static bool ConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}