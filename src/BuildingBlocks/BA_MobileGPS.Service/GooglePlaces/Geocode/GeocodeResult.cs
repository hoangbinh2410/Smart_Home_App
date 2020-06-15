using Newtonsoft.Json;

using System.Collections.Generic;

namespace BA_MobileGPS.Service
{
    public class GeocodeResult
    {
        [JsonProperty("address_components")]
        public List<GeocodeAddressComponent> address_components { get; set; }

        [JsonProperty("formatted_address")]
        public string formatted_address { get; set; }

        [JsonProperty("geometry")]
        public GeocodeGeometry geometry { get; set; }

        [JsonProperty("place_id")]
        public string place_id { get; set; }

        [JsonProperty("types")]
        public List<string> types { get; set; }
    }
}