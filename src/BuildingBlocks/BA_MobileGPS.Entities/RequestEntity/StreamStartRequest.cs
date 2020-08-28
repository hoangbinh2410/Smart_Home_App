using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public abstract class StreamRequestBase
    {
        public int CustomerID { get; set; }

        public string VehicleName { get; set; }

        public int Channel { get; set; }
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
}
