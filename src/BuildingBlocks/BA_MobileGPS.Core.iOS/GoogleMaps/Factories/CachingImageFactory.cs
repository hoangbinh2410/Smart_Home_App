using System.Collections.Concurrent;

using UIKit;

namespace BA_MobileGPS.Core.iOS.Factories
{
    public sealed class CachingImageFactory : IImageFactory
    {
        private readonly ConcurrentDictionary<string, UIImage> _cache = new ConcurrentDictionary<string, UIImage>();

        public UIImage ToUIImage(BitmapDescriptor descriptor)
        {
            var defaultFactory = DefaultImageFactory.Instance;

            if (!string.IsNullOrEmpty(descriptor.Id))
            {
                var cacheEntry = _cache.GetOrAdd(descriptor.Id, _ => defaultFactory.ToUIImage(descriptor));
                return cacheEntry;
            }

            return defaultFactory.ToUIImage(descriptor);
        }
    }
}