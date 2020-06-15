using Newtonsoft.Json;

namespace BA_MobileGPS.Entities
{
    public class BaseCategory
    {
        [JsonProperty("categoryID")]
        public int CategoryID { get; set; }

        [JsonProperty("categotyName")]
        public string CategoryName { get; set; }

        [JsonProperty("pK_ConfigID")]
        public int ConfigID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("valueByLanguage")]
        public string ValueByLanguage { get; set; }

        [JsonProperty("codeLanguage")]
        public string CodeLanguage { get; set; }
    }
}