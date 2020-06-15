using Android.Gms.Maps.Model;

namespace BA_MobileGPS.Core.Droid.Extensions
{
    public static class BoundsExtensions
    {
        public static LatLngBounds ToLatLngBounds(this Bounds self)
        {
            return new LatLngBounds(self.SouthWest.ToLatLng(), self.NorthEast.ToLatLng());
        }
    }
}