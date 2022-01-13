using System;

namespace BA_MobileGPS.Entities
{
    public class ReportQCVN31SpeedRequest : ReportBaseModel
    {
        public int XnCode { get; set; }

        public bool OptionData { get; set; }

        public string Imei { get; set; }
    }
}