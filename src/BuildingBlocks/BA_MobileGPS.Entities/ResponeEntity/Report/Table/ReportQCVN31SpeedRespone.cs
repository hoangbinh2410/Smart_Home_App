using System;

namespace BA_MobileGPS.Entities
{
    public class ReportQCVN31SpeedRespone
    {
        #region Properties

        /// <summary>
        /// Thoi gian su dung trong qua trinh doc LogFile
        /// </summary>
        public DateTime DT { get; set; }

        #endregion Properties

        public string Velocities { get; set; }

        public string Decription { get; set; }

        public int OrderNumber { get; set; }
    }
}