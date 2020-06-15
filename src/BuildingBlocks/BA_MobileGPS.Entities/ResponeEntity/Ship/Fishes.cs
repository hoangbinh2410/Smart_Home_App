using Newtonsoft.Json;

namespace BA_MobileGPS.Entities
{
    public class Fish
    {
        [JsonProperty("fi_id")]
        public int PK_FishID { get; set; }

        [JsonProperty("fi_code")]
        public string Code { get; set; }

        [JsonProperty("fi_name")]
        public string Name { get; set; }
    }
}