using Newtonsoft.Json;

namespace BA_MobileGPS.Service
{
    /// <summary>
    /// The Autocomplete Prediction Term
    /// </summary>
    public class PredictionTerm
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}