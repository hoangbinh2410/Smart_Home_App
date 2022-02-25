using BA_MobileGPS.Utilities.Enums;

using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    /// <summary>
    /// model trả về từ service cho báo cáo động cơ
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  3/31/2019   created
    /// </Modified>
    public class MachineVehicleResponse : ReportBaseResponse
    {
        public string NumberMinutesTurn { get; set; }
        public bool IsMachineOn { get; set; }
        public float FuelConsumed { get; set; }
        public string ShowDateDetailSTR { get; set; }
    }   
}