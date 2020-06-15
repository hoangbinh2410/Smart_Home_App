using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class FishTrip : BaseModel
    {
        public long Id { get; set; }

        [JsonProperty("fi_tr_id")]
        public int PK_FishTrip { get; set; }

        private string shipPlate;

        [JsonProperty("ship_plate")]
        public string ShipPlate { get => shipPlate; set => SetProperty(ref shipPlate, value); }

        private string imei;

        [JsonProperty("imei")]
        public string Imei { get => imei; set => SetProperty(ref imei, value); }

        private DateTimeOffset startTime;

        [JsonProperty("start_time")]
        public DateTimeOffset StartTime { get => startTime; set => SetProperty(ref startTime, value); }

        private double startLatitude;

        [JsonProperty("start_longitude")]
        public double StartLatitude { get => startLatitude; set => SetProperty(ref startLatitude, value); }

        private double startLongitude;

        [JsonProperty("start_latitude")]
        public double StartLongitude { get => startLongitude; set => SetProperty(ref startLongitude, value); }

        private DateTimeOffset endTime;

        [JsonProperty("end_time")]
        public DateTimeOffset EndTime { get => endTime; set => SetProperty(ref endTime, value); }

        private double endLatitude;

        [JsonProperty("end_latitude")]
        public double EndLatitude { get => endLatitude; set => SetProperty(ref endLatitude, value); }

        private double endLongitude;

        [JsonProperty("end_longitude")]
        public double EndLongitude { get => endLongitude; set => SetProperty(ref endLongitude, value); }

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get; set; }

        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public bool IsSynced => LastSynchronizationDate != null && SysLastChangeDate == LastSynchronizationDate;
    }
}