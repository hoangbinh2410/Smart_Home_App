using System;

namespace BA_MobileGPS.Entities
{
    public class FuelReportRequest
    {
        public int CompanyID { get; set; }
        public string ListVehicleID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public FuelStatusEnum SearchType { get; set; }
        public float NumberOfLitter { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool IsAddress { get; set; }
    }

    public enum FuelStatusEnum
    {
        // <summary>
        /// Đổ
        /// </summary>
        Pour = 0,

        /// <summary>
        /// Hút
        /// </summary>
        Absorb = 1,

        /// <summary>
        /// Khả nghi đổ
        /// </summary>
        SuspiciousPour = 2,

        /// <summary>
        /// Khả nghi hút
        /// </summary>
        SuspiciousAbsorb = 3
    }
}