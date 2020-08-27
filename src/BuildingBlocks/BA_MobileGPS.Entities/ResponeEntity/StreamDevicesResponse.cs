using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class StreamDevicesResponse : ResponseStreamBase<List<StreamDevices>>
    {

    }
    public class StreamDevices
    {
        //Giá trị tập hợp các kênh đang có lắp CAM. Mỗi bit đại diện cho 1 kênh CAM. VD: CameraChannel = 3 (0x11) => Có CAM1 và CAM2 đang được lắp. 
        public int CameraChannel { get; set; }

        //Mã xí nghiệp 
        public string CustomerID { get; set; }

        //Trạng thái thiết bị 
        public long DeviceState { get; set; }

        //Loại thiết bị 
        public int DeviceType { get; set; }

        //Mã lỗi của thiết bị 
        public long ErrorCode { get; set; }

        //Phiên bản app BGT 
        public string Firmware { get; set; }

        //Vận tốc GPS 
        public int GpsSpeed { get; set; }

        //IMEI thiết bị 
        public string IMEI { get; set; }

        //Vĩ độ 
        public double Latitude { get; set; }

        //Kinh độ 
        public double Longitude { get; set; }

        //Giá trị tập hợp các kênh CAM đang streaming. Mỗi bit đại diện cho 1 kênh CAM. VD: StreamingChannel = 1 (0x01) => Có CAM1 đang streaming 
        public int StreamingChannel { get; set; }

        //Thời gian cập nhật trạng thái 
        public DateTime UpdatedTime { get; set; }

        //Biển số 
        public string VehicleName { get; set; }

        //Điện áp Acquy (mV) 
        public int Voltage { get; set; }
    }

    public class StreamStartResponse : ResponseStreamBase<List<StreamStart>>
    {

    }

    public class StreamStart
    {
        public int Channel { get; set; }

        public string Link { get; set; }
    }

    public class StreamStopResponse : ResponseStreamBase<bool>
    {

    }

    public class StreamPingResponse : ResponseStreamBase<bool>
    {

    }
}
