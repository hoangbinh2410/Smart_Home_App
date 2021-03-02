using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities
{
    public class AlertOnlineDetailModel : BaseModel
    {
        [JsonProperty("0")]
        public long PK_AlertDetailID { get; set; }

        [JsonProperty("1")]
        public long FK_VehicleID { get; set; }

        [JsonProperty("2")]
        public string VehiclePlate { set; get; }

        [JsonProperty("3")]
        public DateTime StartTime { get; set; }

        [JsonProperty("4")]
        public DateTime? EndTime { get; set; }

        [JsonProperty("5")]
        public short FK_AlertTypeID { get; set; }

        [JsonProperty("6")]
        public string AlertName { set; get; }

        [JsonProperty("7")]
        public int? FK_CompanyID { get; set; }

        [JsonProperty("8")]
        public float? StartLongitude { get; set; }

        [JsonProperty("9")]
        public float? StartLatitude { get; set; }

        [JsonProperty("10")]
        public float? EndLatitude { get; set; }

        [JsonProperty("11")]
        public float? EndLongitude { get; set; }

        [JsonProperty("12")]
        public string Content { get; set; }

        [JsonProperty("13")]
        public int Flags { get; set; }

        [JsonProperty("14")]
        public bool IsRead { get; set; }

        [JsonProperty("15")]
        public bool IsProcessed { get; set; }

        [JsonProperty("16")]
        public string AlertContent { get; set; }

        [JsonProperty("17")]
        public string ProccessContent { get; set; }

        [JsonProperty("18")]
        public int ColorAlert { set; get; }

        [JsonProperty("19")]
        public string ContentEnglish { set; get; }

        [JsonProperty("20")]
        public string IconMobile { set; get; }
    }
}