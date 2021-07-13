using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities
{
    public class CameraRestreamRequest
    {
        public string VehicleNames { get; set; } //Danh sách tên phương tiện cách nhau bởi dấu ,
        public long CustomerId { get; set; }  //XNcode
        public DateTime Date { get; set; } // Ngày cần lây dũ liệu
    }

    public class CameraUploadRequest
    {
        public int CustomerId { get; set; }

        public int Channel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string VehiclePlate { get; set; }

        [JsonIgnore]
        public long VehicleID { get; set; }
    }
}