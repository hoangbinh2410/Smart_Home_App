using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class CaptureImageResponse : ResponseStreamBase<List<CaptureImageData>>
    {
    }

    public class CaptureImageData : BaseModel
    {
        [JsonProperty("v")]
        public string VehiclePlate { get; set; } // VehicleName

        [JsonProperty("c")]
        public DateTime Time { get; set; } // CapturedTime

        [JsonProperty("u")]
        public string Url { get; set; } // Url

        [JsonProperty("a")]
        public float Lat { get; set; } // Latitude

        [JsonProperty("o")]
        public float Lng { get; set; } // Longitude

        [JsonProperty("s")]
        public byte Speed { get; set; } // Speed

        [JsonProperty("k")]
        public byte Channel { get; set; } // Channel

        [JsonProperty("w")]
        public short Width { get; set; } // Width

        [JsonProperty("h")]
        public short Height { get; set; } // Height

        [JsonProperty("y")]
        public short Type { get; set; } // Type

        [JsonProperty("e")]
        public string ExtraInfo { get; set; } // ExtraInfo

        [JsonProperty("z")]
        public string CurrentAddress { get; set; } // Address

        private bool isFavorites;
        public bool IsFavorites { get => isFavorites; set => SetProperty(ref isFavorites, value); }
    }
}