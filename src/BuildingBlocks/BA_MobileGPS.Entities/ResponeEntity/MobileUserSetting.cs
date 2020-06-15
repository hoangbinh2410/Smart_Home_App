using Newtonsoft.Json;

namespace BA_MobileGPS.Entities
{
    public class MobileUserSetting : BaseModel
    {
        public string Name { get; set; }

        [JsonIgnore]
        public string Display { get; set; }

        private string _value;
        public string Value { get => _value; set => SetProperty(ref _value, value); }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsChanged { get; set; }
    }
}