using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class StaticSettings
    {
        public static int AppType { get; set; }
        public static LoginResponse User { get; set; }

        public static string Token { get; set; }

        public static List<HomeMenuItem> ListMenu { get; set; } = new List<HomeMenuItem>();

        public static List<HomeMenuItem> ListMenuOriginGroup { get; set; } = new List<HomeMenuItem>();

        public static List<VehicleOnline> ListVehilceOnline { get; set; }

        public static List<StreamDevices> ListVehilceCamera { get; set; }

        public static List<VehicleDebtMoneyResponse> ListVehilceDebtMoney { get; set; }

        public static List<VehicleFreeResponse> ListVehilceFree { get; set; }

        public static List<AlertTypeModel> ListAlertType { get; set; }

        public static List<Company> ListCompany { get; set; }

        public static DateTime TimeServer { get; set; }

        public static DateTime LastSyncTime { get; set; }

        public static DateTime TimeSleep { get; set; }

        public static void ClearStaticSettings()
        {
            User = null;
            Token = string.Empty;
            ListMenu = new List<HomeMenuItem>();
            ListMenuOriginGroup = new List<HomeMenuItem>();
            ListVehilceOnline = null;
            ListAlertType = null;
            ListVehilceDebtMoney = null;
            ListVehilceCamera = null;
            ListVehilceFree = null;
            ListCompany = null;
            LastSyncTime = DateTime.Now;
        }

        public static bool IsUnauthorized { get; set; }
    }
}