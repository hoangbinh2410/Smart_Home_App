using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class VehicleOnline : BaseModel
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
        public IconCode IconCode { set; get; }

        [JsonProperty("9")]
        public string PrivateCode { set; get; }

        [JsonProperty("10")]
        public byte MessageId { get; set; }

        [JsonProperty("11")]
        public string GroupIDs { set; get; }

        [JsonProperty("12")]
        public bool IsEnableAcc { set; get; }

        [JsonProperty("13")]
        public string CurrentAddress { set; get; }

        [JsonProperty("14")]
        public int DataExt { set; get; }

        [JsonProperty("15")]
        public double TotalKm { set; get; }

        [JsonProperty("16")]
        public DateTime MaturityDate { set; get; }

        [JsonProperty("17")]
        public int Direction { set; get; }

        #region Gói cước tầu cá

        [JsonProperty("19")]
        public string PackageName { get; set; }

        [JsonProperty("20")]
        public string PackageSmsName { get; set; }

        [JsonProperty("21")]
        public int AvailableSmsBlock { get; set; }

        [JsonProperty("22")]
        public DateTime? AvailableSmsDate { get; set; }

        #endregion Gói cước tầu cá

        [JsonProperty("23")]
        public int StopTime { get; set; }

        /// <summary>
        /// Nếu là 1 thì là Ôtô
        /// Nếu là 2 thì là tầu cá
        /// </summary>
        [JsonProperty("24")]
        public byte KindID { get; set; }

        [JsonProperty("25")]
        public string MessageBAP { get; set; }

        [JsonProperty("26")]
        public string MessageDetailBAP { get; set; }

        [JsonProperty("27")]
        public string Imei { set; get; }

        [JsonIgnore]
        public int STT { set; get; }

        [JsonIgnore]
        public int SortOrder { set; get; }

        [JsonIgnore]
        public string IconImage { set; get; }

        [JsonIgnore]
        public string StatusEngineer { set; get; }

        [JsonIgnore]
        public int LostGSMTime => (int)StaticSettings.TimeServer.Subtract(VehicleTime).TotalSeconds;

        ///* Cập nhật lại dữ liệu */
        public void Update(VehicleOnlineMessage message)
        {
            if (message.GPSTime > this.GPSTime && message.VehicleTime > this.VehicleTime)
            {
                this.VehicleId = message.VehicleId;
                this.VehiclePlate = message.VehiclePlate;
                this.Lat = message.Lat;
                this.Lng = message.Lng;
                this.State = message.State;
                this.Velocity = message.Velocity;
                this.GPSTime = message.GPSTime;
                this.VehicleTime = message.VehicleTime;
                this.IconCode = message.IconCode;
                this.Direction = message.Direction;
                this.StopTime = message.StopTime;

                if (message.TotalKm > 0)
                {
                    this.TotalKm = message.TotalKm;
                }
            }
        }
    }
}