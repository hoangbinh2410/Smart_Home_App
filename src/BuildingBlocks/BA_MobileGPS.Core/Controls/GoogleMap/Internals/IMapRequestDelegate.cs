namespace BA_MobileGPS.Core.Internals
{
    public interface IMapRequestDelegate
    {
        void OnMoveToRegionRequest(MoveToRegionMessage m);

        void OnMoveCameraRequest(CameraUpdateMessage m);
    }
}