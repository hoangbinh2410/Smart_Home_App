using Newtonsoft.Json;

namespace BA_MobileGPS.Entities
{
    public class ShipPackage
    {
        [JsonProperty("PK_PackgeShipID")]
        public int PackgeShipID { get; set; }

        [JsonProperty("FK_PackageTypeID")]
        public int PackageTypeID { get; set; }

        [JsonProperty("PackageName")]
        public string PackageName { get; set; }

        [JsonProperty("Frequency")]
        public string Frequency { get; set; }

        [JsonProperty("BlockSMS")]
        public int BlockSMS { get; set; }
    }
}