using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;

namespace BA_MobileGPS.Core.Droid.Factories
{
    public interface IBitmapDescriptorFactory
    {
        AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor);
    }
}