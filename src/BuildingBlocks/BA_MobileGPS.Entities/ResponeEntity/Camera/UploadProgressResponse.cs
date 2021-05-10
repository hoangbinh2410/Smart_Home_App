using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities.ResponeEntity.Camera
{
    public class UploadProgressResponse
    {
        // Convert từ PNC dùng CustomerID
        [JsonProperty("CustomerID")]
        public int XnCode { get; set; }

        // Convert từ PNC dùng VehicleName
        [JsonProperty("VehicleName")]
        public string VehiclePlate { get; set; }

        public byte Channel { get; set; }

        //Tên file đang thực hiện upload
        public string CurrentFile { get; set; }

        //Tổng số file upload không thành công
        public byte ErrorCount { get; set; }

        //Tổng số file đã upload thành công
        public byte FinishCount { get; set; }

        //Tổng số file trong phiên upload
        public byte TotalCount { get; set; }

        //Thời điểm cập nhật dữ liệu tiến trình
        public DateTime UpdatedTime { get; set; }

        public List<string> UploadedFiles { get; set; }
    }
}