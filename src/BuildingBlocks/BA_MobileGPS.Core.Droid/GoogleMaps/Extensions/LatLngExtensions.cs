using Android.Gms.Maps.Model;

namespace BA_MobileGPS.Core.Droid.Extensions
{
    public static class LatLngExtensions
    {
        public static Position ToPosition(this LatLng self)
        {
            return new Position(self.Latitude, self.Longitude);
        }
    }
}