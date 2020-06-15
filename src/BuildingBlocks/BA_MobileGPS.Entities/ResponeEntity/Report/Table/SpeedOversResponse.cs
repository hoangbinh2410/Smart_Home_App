using System;

namespace BA_MobileGPS.Entities
{
    [Serializable]
    public class SpeedOversModel : ReportBaseResponse
    {
        public string PrivateCode { get; set; }

        // Tổng thời gian vi phạm
        public double SpeedOverTotalTime { get; set; }

        // Tổng số m vi phạm
        public double SpeedOverDistance { get; set; }

        //Vmax
        public int Vmax { get; set; }
    }
}