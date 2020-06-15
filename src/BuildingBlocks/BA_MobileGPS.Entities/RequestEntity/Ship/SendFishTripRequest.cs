using Newtonsoft.Json;

using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class SendFishTripRequest
    {
        [JsonProperty("fi_trip_data")]
        public FishTrip FishTrip { get; set; }

        [JsonProperty("list_kind_of_fi_data")]
        public IList<FishTripQuantity> ListFish { get; set; }
    }
}