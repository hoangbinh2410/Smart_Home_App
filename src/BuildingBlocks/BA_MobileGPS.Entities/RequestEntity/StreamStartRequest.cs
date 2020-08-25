using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class StreamStartRequest
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public int Channel { get; set; }
    }

    public class StreamStopRequest
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public int Channel { get; set; }
    }

    public class StreamPingRequest
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public int Channel { get; set; }

        //Thời gian gia hạn quá trình Streaming, mặc định 180s. Đơn vị: second Giá trị: 1 – 600
        public int Duration { get; set; }
    }
}
