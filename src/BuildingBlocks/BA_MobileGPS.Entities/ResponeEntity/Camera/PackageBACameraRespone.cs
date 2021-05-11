using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class PackageBACameraRespone
    {
        public List<PackageBACameraData> Data { get; set; }
        public int ProcessErrorType { get; set; }
        public string ProcessErrorDescription { get; set; }
    }

    public class PackageBACameraData
    {
        public string XNCode { get; set; }
        public string VehiclePlate { get; set; }
        public string IMEI { get; set; }
        public string ServerServiceInfo { get; set; }
        public ServerServiceInfo ServerServiceInfoEnt { get; set; }
        public string CloudServiceInfoInfo { get; set; }
        public CloudServiceInfoInfo CloudServiceInfoInfoEnt { get; set; }
        public string SimCardServiceInfo { get; set; }
        public SimCardServiceInfo SimCardServiceInfoEnt { get; set; }
        public string TCDBServiceInfo { get; set; }
        public TCDBServiceInfo TCDBServiceInfoEnt { get; set; }
    }

    public class ServerServiceInfo
    {
        public string Code { get; set; }
        public int Frequency { get; set; }
        public bool IncludeQcvn31 { get; set; }
        public bool HasImageCapture { get; set; }
        public bool HasVideoStream { get; set; }
    }

    public class CloudServiceInfoInfo
    {
        public string Code { get; set; }
        public int Days { get; set; }
        public int Channels { get; set; }
    }

    public class SimCardServiceInfo
    {
        public string Code { get; set; }
        public string Network { get; set; }
        public int Capacity { get; set; }
        public bool Bandwidth { get; set; }
        public int SmsCount { get; set; }
        public string Notice { get; set; }
    }

    public class TCDBServiceInfo
    {
        public string Code { get; set; }
        public int Frequency { get; set; }
    }
}