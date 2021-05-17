using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class CameraRestreamInfoResponse : BaseResponse<List<CameraRestreamInfo>>
    {
    }

    public class VideoNotUploadResponse : ResponseBaseV2<VideoRestreamInfo>
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

    public class VideoRestreamInfo
    {
        [JsonProperty("v")]
        public string VehicleName { get; set; }

        [JsonProperty("c")]
        public byte Channel { get; set; }

        [JsonProperty("t")]
        public List<VideoUploadTimeInfo> Data { get; set; }

        [JsonIgnore]
        public long VehicleID { get; set; }
    }

    public class VideoTimeInfo
    {
        [JsonProperty("s")]
        public DateTime StartTime { get; set; }

        [JsonProperty("e")]
        public DateTime EndTime { get; set; }
    }

    public class VideoUploadTimeInfo : BaseModel
    {
        [JsonProperty("s")]
        public DateTime StartTime { get; set; }

        [JsonProperty("e")]
        public DateTime EndTime { get; set; }

        [JsonProperty("f")]
        public string FileName { get; set; }

        [JsonProperty("u")]
        public bool IsUploaded { get; set; }

        private bool isSelected = true;

        [JsonIgnore]
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        private string note;

        [JsonIgnore]
        public string Note { get => note; set => SetProperty(ref note, value); }
    }

    public class CameraRestreamUploadResponse : BaseResponse<List<CameraRestreamUploadInfo>>
    {
    }

    public class CameraRestreamUploadInfo
    {
        [JsonProperty("v")]
        public string VehicleName { get; set; }

        [JsonProperty("c")]
        public byte Channel { get; set; }

        [JsonProperty("d")]
        public List<VideoUploadInfo> Data { get; set; }
    }

    public class VideoUploadInfo : BaseModel
    {
        [JsonProperty("s")]
        public DateTime StartTime { get; set; }

        [JsonProperty("t")]
        public int Duration { get; set; }

        [JsonProperty("f")]
        public string FileName { get; set; }

        [JsonProperty("l")]
        public string Link { get; set; }

        [JsonProperty("i")]
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public byte Channel { get; set; }

        private bool isSelected = false;

        [JsonIgnore]
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
    }

    public class DeviceTabVideoInfoResponse : BaseResponse<List<RestreamVideoTimeInfo>>
    {
    }

    public class RestreamVideoTimeInfo : VideoTimeInfo
    {
        [JsonProperty("i")]
        public string Image { get; set; }

        [JsonProperty("c")]
        public byte Channel { get; set; }

        [JsonProperty("d")]
        public byte Duration { get; set; } = 0;
    }
}