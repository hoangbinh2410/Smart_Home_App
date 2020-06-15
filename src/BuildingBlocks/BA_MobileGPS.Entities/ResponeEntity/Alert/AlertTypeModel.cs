using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class AlertTypeModel
    {
        [JsonProperty("1")]
        public int PK_AlertTypeID { set; get; }

        [JsonProperty("2")]
        public string Name { set; get; }

        [JsonProperty("3")]
        public int ColorAlert { set; get; }

        [JsonProperty("4")]
        public Guid FK_UserID { set; get; }

        [JsonProperty("5")]
        public string NameEng { set; get; }

        [JsonProperty("6")]
        public string AlertTypeIDs { set; get; }

        [JsonProperty("7")]
        public string IconMobile { set; get; }
    }
}