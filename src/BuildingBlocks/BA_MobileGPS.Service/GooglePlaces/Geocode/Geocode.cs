using Newtonsoft.Json;

using System.Collections.Generic;

namespace BA_MobileGPS.Service
{
    public class Geocode
    {
        [JsonProperty("results")]
        public List<GeocodeResult> results { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }
    }
}