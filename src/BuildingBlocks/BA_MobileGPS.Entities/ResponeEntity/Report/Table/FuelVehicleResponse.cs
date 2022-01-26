using BA_MobileGPS.Utilities.Enums;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class FuelVehicleResponse
    {
        public bool State { get; set; }

        public ResponseEnum ErrorCode { get; set; }

        public List<FuelVehicleModel> ListFuelReport { get; set; }
    }

    public class FuelVehicleModel : ReportBaseResponse
    {
        /// <summary>
        /// Gets or Sets PreviousValue
        /// </summary>
        public int PreviousValue { get; set; }

        /// <summary>
        /// Gets or Sets AfterValue
        /// </summary>
        public int AfterValue { get; set; }

        /// <summary>
        /// Gets or Sets PreviousKm
        /// </summary>
        public double PreviousKm { get; set; }

        /// <summary>
        /// Gets or Sets AfterKm
        /// </summary>
        public double AfterKm { get; set; }

        /// <summary>
        /// Gets or Sets Longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or Sets Latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or Sets PrivateCode
        /// </summary>
        public string PrivateCode { get; set; }

        /// <summary>
        /// Current address of vehicle
        /// </summary>
        public string CurrentAddress { get; set; }

        /// <summary>
        /// Gets number of Previous fuel liter
        /// </summary>
        public float PreviousLit { get; set; }

        /// <summary>
        /// Gets number of After fuel liter
        /// </summary>
        public float AfterLit { get; set; }

        /// <summary>
        /// Number of fuel liter that change
        /// </summary>
        public float ChangingFuel { get; set; }

        [JsonIgnore]
        public string FuelStatus { get; set; }
    }
}