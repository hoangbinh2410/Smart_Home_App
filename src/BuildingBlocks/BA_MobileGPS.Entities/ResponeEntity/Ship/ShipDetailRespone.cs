using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class ShipDetailRespone : BaseModel
    {
        /// <summary>
        /// IMEI thiết bị
        /// </summary>
        [JsonProperty("1")]
        public string PrivateCode { get; set; }

        /// <summary>
        /// IMEI thiết bị
        /// </summary>
        [JsonProperty("2")]
        public string IMEI { get; set; }

        /// <summary>
        /// Thời gian theo GPS
        /// </summary>
        [JsonProperty("3")]
        public DateTime GPSTime { get; set; }

        /// <summary>
        /// Vận tốc theo GPS
        /// </summary>
        [JsonProperty("4")]
        public int VelocityGPS { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        [JsonProperty("5")]
        public double Latitude { get; set; }

        /// <summary>
        /// Kinh độ
        /// </summary>
        [JsonProperty("6")]
        public double Longtitude { get; set; }

        /// <summary>
        /// Km tích lũy trong ngày
        /// </summary>
        [JsonProperty("7")]
        public double Km { get; set; }

        /// <summary>
        /// Tên chủ tàu
        /// </summary>
        [JsonProperty("8")]
        public string ShipOwnerName { get; set; }

        /// <summary>
        /// Số điện thoại chủ tàu
        /// </summary>
        [JsonProperty("9")]
        public string ShipOwnerPhoneNumber { get; set; }

        /// <summary>
        /// tên thuyền trưởng
        /// </summary>
        [JsonProperty("10")]
        public string ShipCaptainName { get; set; }

        /// <summary>
        /// Số điện thoại thuyền trưởng
        /// </summary>
        [JsonProperty("11")]
        public string ShipCaptainPhoneNumber { get; set; }

        /// <summary>
        /// Số lượng thuyền viên
        /// </summary>
        [JsonProperty("12")]
        public int? ShipMembers { get; set; }

        /// <summary>
        /// Cảng xuất phát
        /// </summary>
        [JsonProperty("13")]
        public string PortDeparture { get; set; }

        /// <summary>
        /// Ngày hết hạn SMS
        /// </summary>
        [JsonProperty("14")]
        public DateTime? AvailableSmsDate { get; set; }

        /// <summary>
        /// Ngày hết hạn SMS
        /// </summary>
        [JsonProperty("15")]
        public DateTime? MCExpried { get; set; }

        [JsonProperty("16")]
        public byte MessageId { get; set; }

        /// <summary>
        ///  Đánh dấu là Tầu cá hay Xe
        /// </summary>
        [JsonProperty("17")]
        public byte KindID { get; set; }

        /// <summary>
        /// Thông báo MessageBAP
        /// </summary>
        [JsonProperty("18")]
        public string MessageBAP { get; set; }

        [JsonIgnore]
        public string Address { get; set; }

        [JsonIgnore]
        public int LostGSMTime => (int)StaticSettings.TimeServer.Subtract(GPSTime).TotalSeconds;

        ///* Cập nhật lại dữ liệu */
        public void Update(VehicleOnline message)
        {
            Latitude = message.Lat;
            Longtitude = message.Lng;
            VelocityGPS = message.State;
            GPSTime = message.GPSTime;
        }
    }
}