using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class StreamDevice
    {
        public int Acquy { get; set; }
        public int Band { get; set; }
        public int CSQ { get; set; }
        [JsonProperty("Cameras")]
        public List<CameraChannel> Channels { get; set; }
        public string Customer { get; set; }
        public string IMEI { get; set; }
        public int Lat { get; set; }
        public int Lng { get; set; }
        public int Net { get; set; }
        public int State { get; set; }
        public List<Storage> Storages { get; set; }
        public DateTime Time { get; set; }
        public string Vehicle { get; set; }
        public int NetType { get; set; }
        public Coreboard Coreboard { get; set; }
    }

    public class CameraChannel
    {
        public int State { get; set; }
        public int CameraStatus { get; set; }
        public int Channel { get; set; }
        public int FPS { get; set; }
        public bool Record { get; set; }
        public bool Stream { get; set; }
        public bool IsStreaming { get; set; }
        public int StreamingTimeout { get; set; }
        public int StreamingTotal { get; set; }
    }

    public class Storage
    {
        public bool IsInserted { get; set; }
        public long Free { get; set; }
        public long Total { get; set; }
        public int Type { get; set; }
    }

    public class Coreboard
    {
        public int RamFreeSize { get; set; }
        public int StorageFreeSize { get; set; }
        public double Temperature { get; set; }
    }
}