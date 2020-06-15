namespace BA_MobileGPS.Entities
{
    public class MachineVehcleRequest : ReportBaseModel
    {
        public bool? State { get; set; }
        public int NumberOfMinutes { get; set; }
        public string SortField { get; set; }
    }
}