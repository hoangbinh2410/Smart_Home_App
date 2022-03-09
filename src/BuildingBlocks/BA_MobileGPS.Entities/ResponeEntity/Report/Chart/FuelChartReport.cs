using BA_MobileGPS.Utilities.Enums;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class FuelChartReport
    {
        public FuelChartReport()
        {
        }

        [JsonProperty("Time")]
        public DateTime Time { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }

        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("CurrentAddress")]
        public string CurrentAddress { get; set; }

        [JsonProperty("VelocityGPS")]
        public int VelocityGPS { get; set; }

        [JsonProperty("MachineStatus")]
        public int MachineStatus { get; set; }

        [JsonProperty("Km")]
        public double Km { get; set; }

        [JsonProperty("NumberOfLiters")]
        public List<double> NumberOfLiters { get; set; }
    }

    public class FuelChartDisplay
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime Time { get; set; }

        public string CurrentAddress { get; set; }

        public int VelocityGPS { get; set; }

        public string MachineStatus { get; set; }

        public double Km { get; set; }

        public double NumberOfLiters { get; set; }
    }
}