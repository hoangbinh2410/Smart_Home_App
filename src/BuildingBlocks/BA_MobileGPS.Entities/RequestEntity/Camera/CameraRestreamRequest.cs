using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities 
{ 
   public class CameraRestreamRequest
    {
        public string VehicleNames { get; set; } //Danh sách tên phương tiện cách nhau bởi dấu ,
        public long customerId { get; set; }  //XNcode
        public DateTime Date { get; set; } // Ngày cần lây dũ liệu
    }

    public class StartRestreamRequest
    {
        public int CustomerID { get; set; }
        public string VehicleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Channel { get; set; }
    }

    public class StopRestreamRequest
    {
        public int CustomerID { get; set; }
        public string VehicleName { get; set; }
        public int Channel { get; set; }
    }
}
