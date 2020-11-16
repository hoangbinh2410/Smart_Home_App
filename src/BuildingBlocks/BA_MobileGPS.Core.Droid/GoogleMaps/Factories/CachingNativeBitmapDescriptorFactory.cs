using BA_MobileGPS.Core.Droid.Factories;

using System.Collections.Concurrent;

using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;

namespace BA_MobileGPS.Core.Droid
{
    public sealed class CachingNativeBitmapDescriptorFactory : IBitmapDescriptorFactory
    {
        private readonly ConcurrentDictionary<string, AndroidBitmapDescriptor> _cache
            = new ConcurrentDictionary<string, AndroidBitmapDescriptor>();

        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            var defaultFactory = DefaultBitmapDescriptorFactory.Instance;

            if (!string.IsNullOrEmpty(descriptor.Id))
            {
                var cacheEntry = _cache.GetOrAdd(descriptor.Id, _ => defaultFactory.ToNative(descriptor));
                return cacheEntry;
            }

            return defaultFactory.ToNative(descriptor);
        }
    }
}