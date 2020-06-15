using Newtonsoft.Json;

namespace BA_MobileGPS.Entities
{
    public class Vehicle
    {
        [JsonProperty("vehicleId")]
        public long VehicleId { set; get; }

        [JsonProperty("vehiclePlate")]
        public string VehiclePlate { set; get; }

        [JsonProperty("privateCode")]
        public string PrivateCode { set; get; }

        [JsonProperty("groupIDs")]
        public string GroupIDs { set; get; }

        [JsonProperty("Imei")]
        public string Imei { set; get; }
    }
}