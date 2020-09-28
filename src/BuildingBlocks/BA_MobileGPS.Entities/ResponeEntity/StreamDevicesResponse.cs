using Newtonsoft.Json;
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
        public string AdminAppVersion { get; set; }

        public int BatteryVoltage { get; set; }

        public string CameraAppVersion { get; set; }

        public List<CameraChannel> CameraChannels { get; set; }

        public Coreboard Coreboard { get; set; }

        public string XnCode { get; set; }

        public string DeviceID { get; set; }

        public string DeviceIP { get; set; }

        public int DeviceState { get; set; }

        public DateTime DeviceTime { get; set; }

        public int DeviceType { get; set; }

        public int ErrorCode { get; set; }

        public string Firmware { get; set; }

        public int GpsSpeed { get; set; }

        public string IMEI { get; set; }

        public string IMEI2 { get; set; }

        public DateTime LastCapturedTime { get; set; }

        public DateTime LastStreamingTime { get; set; }

        //Vĩ độ 
        public double Latitude { get; set; }

        //Kinh độ 
        public double Longitude { get; set; }

        public int NetworkBand { get; set; }

        public int NetworkType { get; set; }

        public string SIMIMEI { get; set; }

        public string SIMIMEI2 { get; set; }

        public List<StorageDevices> StorageDevices { get; set; }

        //Thời gian cập nhật trạng thái 
        public DateTime UpdatedTime { get; set; }

        //Biển số 
        public string VehiclePlate { get; set; }
    }

    public class CameraChannel
    {
        public int CameraStatus { get; set; }

        public int Channel { get; set; }

        public int ErrorCode { get; set; }

        public bool IsPlug { get; set; }

        public bool IsRecording { get; set; }

        public bool IsStreaming { get; set; }
    }

    public class Coreboard
    {
        public int RamFreeSize { get; set; }

        public int StorageFreeSize { get; set; }

        public double Temperature { get; set; }
    }

    public class StorageDevices
    {
        public int ErrorCode { get; set; }

        public int FreeSize { get; set; }

        public bool IsInserted { get; set; }

        public object TotalSize { get; set; }

        public int Type { get; set; }
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

    public class CaptureImageGroup
    {
        public string VehiclePlate { get; set; }

        public List<CaptureImageData> Data { get; set; }
    }

    public class CaptureImageResponse : ResponseStreamBase<List<CaptureImageData>>
    {

    }

    public class CaptureImageModel : ResponseStreamBase<List<CaptureImageGroup>>
    {

    }

    public class CaptureImageData
    {
        [JsonProperty("v")]
        public string VehiclePlate { get; set; } // VehicleName

        [JsonProperty("c")]
        public DateTime Time { get; set; } // CapturedTime

        [JsonProperty("u")]
        public string Url { get; set; } // Url 

        [JsonProperty("a")]
        public float Lat { get; set; } // Latitude 

        [JsonProperty("o")]
        public float Lng { get; set; } // Longitude 

        [JsonProperty("s")]
        public byte Speed { get; set; } // Speed 

        [JsonProperty("k")]
        public byte Channel { get; set; } // Channel 

        [JsonProperty("w")]
        public short Width { get; set; } // Width

        [JsonProperty("h")]
        public short Height { get; set; } // Height 

        [JsonProperty("y")]
        public short Type { get; set; } // Type 

        [JsonProperty("e")]
        public string ExtraInfo { get; set; } // ExtraInfo

        [JsonProperty("z")]
        public string CurrentAddress { get; set; } // Address
    }
}
