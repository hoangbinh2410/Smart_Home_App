using System.ComponentModel;

namespace BA_MobileGPS.Utilities.Enums
{
    public enum StatusMachineReportEnum
    {
        [Description("All")]
        All = 0,

        [Description("TurnOnMachine")]
        TurnOnMachine = 1,

        [Description("TurnOffMachine")]
        TurnOffMachine = 2
    }
}