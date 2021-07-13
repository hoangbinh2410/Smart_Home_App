namespace BA_MobileGPS.Entities
{
    public class SetHotspotRequest : CameraBaseRequest
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public bool Enable { get; set; }
    }
}