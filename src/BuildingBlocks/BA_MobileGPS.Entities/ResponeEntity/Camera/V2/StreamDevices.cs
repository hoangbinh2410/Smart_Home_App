using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class StreamDevice
    {
        public int Acquy { get; set; }
        public int Band { get; set; }
        public int CSQ { get; set; }
        public List<CameraChannelv2> Cameras { get; set; }
        public string Customer { get; set; }
        public string IMEI { get; set; }
        public int Lat { get; set; }
        public int Lng { get; set; }
        public int Net { get; set; }
        public int State { get; set; }
        public List<Storage> Storages { get; set; }
        public DateTime Time { get; set; }
        public string Vehicle { get; set; }
    }

    public class CameraChannelv2
    {
        public int CameraStatus { get; set; }
        public int Channel { get; set; }
        public int FPS { get; set; }
        public bool Record { get; set; }
        public bool Stream { get; set; }
    }

    public class Storage
    {
        public long Free { get; set; }
        public long Total { get; set; }
        public int Type { get; set; }
    }
}