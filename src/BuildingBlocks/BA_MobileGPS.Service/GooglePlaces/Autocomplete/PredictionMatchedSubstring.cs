using Newtonsoft.Json;

namespace BA_MobileGPS.Service
{
    /// <summary>
    /// The Autocomplete Prediction Substring
    /// </summary>
    public class PredictionMatchedSubstring
    {
        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }
    }
}