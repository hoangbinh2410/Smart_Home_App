using Newtonsoft.Json;

using System.Collections.Generic;

namespace BA_MobileGPS.Service
{
    /// <summary>
	/// The Autocomplete Prediction Collection
	/// </summary>
	public class Predictions
    {
        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("predictions")]
        public List<Prediction> predictions { get; set; }
    }
}