using Newtonsoft.Json;

using System.Collections.Generic;

namespace BA_MobileGPS.Service
{
    public class GeocodeAddressComponent
    {
        [JsonProperty("long_name")]
        public string long_name { get; set; }

        [JsonProperty("short_name")]
        public string short_name { get; set; }

        [JsonProperty("types")]
        public List<string> types { get; set; }
    }
}