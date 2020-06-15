using Newtonsoft.Json;
using System;

namespace BA_MobileGPS.Entities
{
    [Serializable]
    public class SendEngineRespone
    {
        [JsonProperty("Error")]
        public ErrorSendEngine Error { get; set; }

        [JsonProperty("State")]
        public bool State { get; set; }
    }

    public class ErrorSendEngine
    {
        public ErrorSendEngineEnum Code { get; set; }
        public string InternalMessage { get; set; }
        public string UserMessage { get; set; }
    }

    public enum ErrorSendEngineEnum
    {
        Success = 0,
        WrongKey = 1,
        DeviceNotOnline = 2,
        ParmsNotValid = 3,
        Unknown = 100
    }

    public class ActionOnOffMachineLogViewModel
    {
        public int OrderNumber { get; set; }
        public Guid FK_UserID { get; set; }
        public string VehiclePate { get; set; }
        public DateTime TimeAction { get; set; }
        public bool? Action { get; set; }
        public string ActionStr { get; set; }
        public byte? State { get; set; }
        public string StateStr { get; set; }
    }
}