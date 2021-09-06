namespace BA_MobileGPS.Entities
{
    public class PlaybackStopRequest : CameraBaseRequest
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public int Channel { get; set; }
    }
}