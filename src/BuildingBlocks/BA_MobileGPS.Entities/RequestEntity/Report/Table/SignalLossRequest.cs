namespace BA_MobileGPS.Entities
{
    public class SignalLossRequest : ReportBaseModel
    {

        public string VehicleID { get; set; }

        public int MinuteLossSignal { get; set; }

        public byte Type { get; set; }
    }
}
