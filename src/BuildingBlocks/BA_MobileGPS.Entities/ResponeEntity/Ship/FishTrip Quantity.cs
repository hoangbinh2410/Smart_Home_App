using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class FishTripQuantity : BaseModel
    {
        public long Id { get; set; }

        public long Parent { get; set; }

        [JsonProperty("fk_fi_tr_id")]
        public int FK_FishTripID { get; set; }

        [JsonProperty("fk_fi_id")]
        public int FK_FishID { get; set; }

        [JsonIgnore]
        public string FishName { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        private bool isDeleted;

        [JsonProperty("is_deleted")]
        public bool IsDeleted { get => isDeleted; set => SetProperty(ref isDeleted, value); }

        public DateTimeOffset? LastSynchronizationDate { get; set; }

        public DateTimeOffset? SysLastChangeDate { get; set; }

        public bool IsSynced => SysLastChangeDate == LastSynchronizationDate;
    }
}