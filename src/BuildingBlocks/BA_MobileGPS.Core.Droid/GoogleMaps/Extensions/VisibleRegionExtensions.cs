using Android.Gms.Maps.Model;

using BA_MobileGPS.Core.Droid.Extensions;

namespace BA_MobileGPS.Core.Droid
{
    public static class VisibleRegionExtensions
    {
        public static MapRegion ToRegion(this VisibleRegion visibleRegion)
        {
            return new MapRegion(
                visibleRegion.NearLeft.ToPosition(),
                visibleRegion.NearRight.ToPosition(),
                visibleRegion.FarLeft.ToPosition(),
                visibleRegion.FarRight.ToPosition()
            );
        }
    }
}