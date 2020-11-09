﻿using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities
{
    public class Vehicle
    {
        [JsonProperty("vehicleId")]
        public long VehicleId { set; get; }

        [JsonProperty("vehiclePlate")]
        public string VehiclePlate { set; get; }

        [JsonProperty("privateCode")]
        public string PrivateCode { set; get; }

        [JsonProperty("groupIDs")]
        public string GroupIDs { set; get; }

        [JsonProperty("Imei")]
        public string Imei { set; get; }

        [JsonIgnore]
        public string IconImage { set; get; }

        [JsonIgnore]
        public int Velocity { set; get; }

        [JsonIgnore]
        public DateTime VehicleTime { set; get; }

        [JsonIgnore]
        public int SortOrder { set; get; }
    }
}