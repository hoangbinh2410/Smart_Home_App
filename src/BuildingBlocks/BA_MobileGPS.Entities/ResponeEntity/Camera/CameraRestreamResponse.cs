using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public class CameraRestreamInfoResponse: BaseResponse<List<CameraRestreamInfo>>
    {

    }

    public class CameraRestreamInfo
    {
        [JsonProperty("v")]
        public string VehicleName { get; set; }
        [JsonProperty("c")]
        public byte Channel { get; set; }
        [JsonProperty("t")]
        public List<AppVideoTimeInfor> Data { get; set; } 
    }

    public class VideoTimeInfo
    {
        [JsonProperty("s")]
        public DateTime StartTime { get; set; }
        [JsonProperty("e")]
        public DateTime EndTime { get; set; }
    }

    public class AppVideoTimeInfor : VideoTimeInfo
    {
        public byte Channel { get; set; }
    }


    public class CameraRestreamUploadResponse : BaseResponse<List<CameraRestreamUpload>>
    {

    }


    public class CameraRestreamUpload
    {
        [JsonProperty("v")]
        public string VehicleName { get; set; }
        [JsonProperty("c")]
        public byte Channel { get; set; }
        [JsonProperty("d")]
        public List<VideoUpload> Data { get; set; } 
    }

    public class VideoUpload
    {
        [JsonProperty("s")]
        public DateTime StartTime { get; set; }
        [JsonProperty("t")]
        public int Duration { get; set; }
        [JsonProperty("f")]
        public string FileName { get; set; }
        [JsonProperty("l")]
        public string Link { get; set; }
    }

}
