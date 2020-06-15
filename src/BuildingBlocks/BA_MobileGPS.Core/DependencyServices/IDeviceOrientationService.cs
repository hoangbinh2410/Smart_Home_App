namespace BA_MobileGPS.Core
{
    public enum Orientation
    {
        Portrait,
        Landscape,
        Both
    }

    public interface IDeviceOrientationService
    {
        void ChangeOrientation(Orientation orientation);
    }
}