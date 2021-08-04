using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
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

        [JsonProperty("u")]
        public bool IsUploaded { get; set; }

        private bool isSelected = true;

        [JsonIgnore]
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }

        private VideoUploadStatus status = VideoUploadStatus.NotUpload;

        [JsonIgnore]
        public VideoUploadStatus Status { get => status; set => SetProperty(ref status, value); }
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

        [JsonProperty("f")]
        public string FileName { get; set; }

        [JsonProperty("t")]
        public int Duration { get; set; }

        [JsonProperty("l")]
        public string Link { get; set; }

        [JsonIgnore]
        public byte Channel { get; set; }

        [JsonIgnore]
        public string VehicleName { get; set; }

        [JsonIgnore]
        public DateTime EndTime { get; set; }

        private VideoUploadStatus status = VideoUploadStatus.Uploaded;

        [JsonIgnore]
        public VideoUploadStatus Status { get => status; set => SetProperty(ref status, value); }
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

    public class VideoUpload : BaseModel
    {
        public long VehicleID { get; set; }
        public string VehicleName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Channel { get; set; }

        private VideoUploadStatus status = VideoUploadStatus.Uploaded;
        public VideoUploadStatus Status { get => status; set => SetProperty(ref status, value); }
    }

    public enum VideoUploadStatus
    {
        WaitingUpload = 0,
        Uploading = 1,
        Uploaded = 2,
        UploadErrorTimeout = 3,
        UploadErrorCancel = 4,
        UploadErrorDevice = 5,
        NotUpload = 6,
    }
}