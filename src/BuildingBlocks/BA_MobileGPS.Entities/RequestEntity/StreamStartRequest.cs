namespace BA_MobileGPS.Entities
{
    public abstract class StreamRequestBase
    {
        public string xnCode { get; set; }

        public string VehiclePlate { get; set; }

        public int Channel { get; set; }
        public string IMEI { get; set; }
    }

    public class StreamStartRequest : StreamRequestBase
    {
    }

    public class StreamStopRequest : StreamRequestBase
    {
    }

    public class StreamPingRequest : StreamRequestBase
    {
        //Thời gian gia hạn quá trình Streaming, mặc định 180s. Đơn vị: second Giá trị: 1 – 600
        public int Duration { get; set; }
    }

    public class StreamImageRequest
    {
        public int xnCode { get; set; }

        public string VehiclePlates { get; set; }
    }
}