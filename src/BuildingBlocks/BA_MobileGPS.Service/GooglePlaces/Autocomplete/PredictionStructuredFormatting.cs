using Newtonsoft.Json;

using System.Collections.Generic;

namespace BA_MobileGPS.Service
{
    /// <summary>
    /// The Autocomplete Prediction Substring
    /// </summary>
    public class PredictionStructuredFormatting
    {
        [JsonProperty("main_text")]
        public string MainText { get; set; }

        [JsonProperty("main_text_matched_substrings")]
        public List<PredictionMatchedSubstring> main_text_matched_substrings { get; set; }

        [JsonProperty("secondary_text")]
        public string SecondaryText { get; set; }
    }
}