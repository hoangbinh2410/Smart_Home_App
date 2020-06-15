using UIKit;

namespace BA_MobileGPS.Core.iOS.Factories
{
    public interface IImageFactory
    {
        UIImage ToUIImage(BitmapDescriptor descriptor);
    }
}