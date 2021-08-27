namespace BA_MobileGPS.Entities
{
    public class VehicleCamera
    {
        public long VehicleId { set; get; }

        public string VehiclePlate { set; get; }

        public string PrivateCode { set; get; }

        public string Imei { set; get; }

        public bool IsQcvn31 { set; get; }

        public bool HasImage { set; get; }

        public bool HasVideo { set; get; }

        public int Channel { get; set; }
    }
}