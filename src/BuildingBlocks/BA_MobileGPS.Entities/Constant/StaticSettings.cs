using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class StaticSettings
    {
        public static LoginResponse User { get; set; }

        public static string Token { get; set; }
        //Bottom TabItem in main page except home & account tab
        public static List<HomeMenuItem> ListMenu { get; set; } = new List<HomeMenuItem>();

        // Lưu menu khi đc trả về từ api và lọc qua permisison
        public static List<HomeMenuItem> ListMenuOriginGroup { get; set; } = new List<HomeMenuItem>();

        public static List<VehicleOnline> ListVehilceOnline { get; set; }

        public static List<VehicleDebtMoneyResponse> ListVehilceDebtMoney { get; set; }

        public static List<VehicleFreeResponse> ListVehilceFree { get; set; }

        public static List<AlertTypeModel> ListAlertType { get; set; }

        public static List<Company> ListCompany { get; set; }

        public static DateTime TimeServer { get; set; }

        public static void ClearStaticSettings()
        {
            User = null;
            ListVehilceOnline = null;
            ListAlertType = null;
            ListVehilceDebtMoney = null;
            ListVehilceFree = null;
            ListCompany = null;
        }
    }
}