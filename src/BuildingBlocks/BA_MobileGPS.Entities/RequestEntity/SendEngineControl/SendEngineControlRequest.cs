using System;

namespace BA_MobileGPS.Entities
{
    public class SendEngineControlRequest
    {
        public Guid FK_UserID { get; set; }
        public string Imei { get; set; }
        public string VehiclePlate { get; set; }
        public string Command { get; set; }
        public int XNCode  { get; set; }
    }

    public class ActionOnOffMachineLogRequest
    {
        public Guid FK_UserID { get; set; }

        public string VehiclePlate { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}