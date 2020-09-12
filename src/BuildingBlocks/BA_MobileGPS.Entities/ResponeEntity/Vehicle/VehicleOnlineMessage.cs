using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleOnlineMessage
    {
        [JsonProperty("0")]
        public long VehicleId { set; get; }

        [JsonProperty("1")]
        public string VehiclePlate { set; get; }

        [JsonProperty("2")]
        public float Lat { set; get; }

        [JsonProperty("3")]
        public float Lng { set; get; }

        [JsonProperty("4")]
        public int State { set; get; }

        [JsonProperty("5")]
        public int Velocity { set; get; }

        [JsonProperty("6")]
        public DateTime GPSTime { get; set; }

        [JsonProperty("7")]
        public DateTime VehicleTime { get; set; }

        [JsonProperty("8")]
        public IconCode IconCode { get; set; }

        [JsonProperty("9")]
        public string PrivateCode { set; get; }

        [JsonProperty("10")]
        public DateTime LastTimeMove { get; set; }

        [JsonProperty("11")]
        public int VelocityMechanical { get; set; }

        [JsonProperty("12")]
        public bool IsLocked { get; set; }

        [JsonProperty("13")]
        public bool IsShow { get; set; }

        [JsonProperty("14")]
        public int Flags { get; set; }

        [JsonProperty("15")]
        public int DataExt { get; set; }

        [JsonProperty("16")]
        public int Direction { set; get; }

        [JsonProperty("17")]
        public int XNCode { get; set; }

        [JsonProperty("18")]
        public int StopTime { get; set; }

        [JsonProperty("19")]
        public double TotalKm { get; set; }

        [JsonIgnore]
        public bool IsEnableAcc { set; get; }
    }
}