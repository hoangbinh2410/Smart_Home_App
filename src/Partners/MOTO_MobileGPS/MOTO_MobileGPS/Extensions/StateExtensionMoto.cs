using BA_MobileGPS.Core;
using BA_MobileGPS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOTO_MobileGPS.Extensions
{
    public static class StateExtensionMoto
    {
        /* Trạng thái gps */

        public static bool IsVehicleOffline(DateTime gpstime)
        {
            if (StaticSettings.TimeServer.Subtract(gpstime).TotalMinutes >= MobileSettingHelper.TimeVehicleOffline)//Nếu xe mất GPS
            {
                return true;
            }
            return false;
        }
    }
}
