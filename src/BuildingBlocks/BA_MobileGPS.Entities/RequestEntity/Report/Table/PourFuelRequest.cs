using System;

namespace BA_MobileGPS.Entities
{
    public class FuelReportRequest : ReportBaseModel
    {       
        public FuelStatusEnum SearchType { get; set; }
        public float NumberOfLitter { get; set; }
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