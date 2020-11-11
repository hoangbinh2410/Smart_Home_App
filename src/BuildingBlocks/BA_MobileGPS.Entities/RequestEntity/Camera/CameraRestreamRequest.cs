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
}
