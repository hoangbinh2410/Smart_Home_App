using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class AlertMaskModel
    {
        public byte WarningType { set; get; }

        public long VehicleId { set; get; }

        public string VehiclePlate { set; get; }

        public DateTime TimeStart { set; get; }

        public double Latitude { set; get; }

        public double Longitude { set; get; }

        public string WarningContent { set; get; }

        public string VehicleNo { set; get; }

        public int CompanyID { set; get; }

        public string Url { set; get; }

        public string CurrentAddress { get; set; }

        public int CountUserNotMask { get; set; }

        public int CountUserUseMask { get; set; }

        public List<int> ListMask { get; set; }

        public List<int> ListNoMask { get; set; }
    }
}