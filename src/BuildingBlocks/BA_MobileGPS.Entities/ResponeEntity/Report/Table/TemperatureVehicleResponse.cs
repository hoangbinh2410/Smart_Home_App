using BA_MobileGPS.Utilities.Enums;

using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class TemperatureVehicleResponse : ReportBaseResponse
    {
        private float? StartKm { get; set; }

        private float? EndKm { get; set; }

        public float? Temperature { get; set; }

        public byte? NumberOfSensors { get; set; }

        public string ShowDateDetailSTR { get; set; }

        public string NumberOfSensorsSTR { get; set; }

        private DateTime? CreatedDate { get; set; }

        public float Kilometer { get; set; }
    }

    public class TemperatureResponse
    {
        public bool State { get; set; }
        public ResponseEnum ErrorCode { get; set; }
        public List<TemperatureVehicleResponse> ListReportTemperature { get; set; }
        public int Count { get; set; }
    }
}