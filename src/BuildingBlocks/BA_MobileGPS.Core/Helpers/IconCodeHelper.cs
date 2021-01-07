using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities.Extensions;

namespace BA_MobileGPS.Core.Helpers
{
    public class IconCodeHelper
    {
        public static string GetIconCarFromStates(IconCode iconCode, IconColor iconColor)
        {
            // Nếu không có iconCode thì lấy ic_car_traking làm mặc định
            string icon = "ball_blue.png";
            if (string.IsNullOrEmpty(iconCode.ToDescription()) || string.IsNullOrEmpty(iconCode.ToDescription()))
            {
                return icon;
            }
            else
            {
                if (IconCode.Other.ToDescription().Equals(iconCode.ToDescription()))
                {
                    icon = iconCode.ToDescription() + "_" + iconColor.ToDescription();
                }
                else
                {
                    icon = iconCode.ToDescription() + "_" + iconColor.ToDescription();
                }
            }
            return icon;
        }

        /* Tính màu cho marker */

        public static string GetMarkerResource(VehicleOnline carInfo)
        {
            if (App.AppType == AppType.VMS && StateVehicleExtension.IsSatelliteError(carInfo))
            {
                return "ic_errorgps.png";
            }

            /* Vehicle time mất > 150 phút -> mất GSM */
            if (StateVehicleExtension.IsLostGSM(carInfo.VehicleTime) || StaticSettings.TimeServer.Subtract(carInfo.GPSTime).TotalMinutes > CompanyConfigurationHelper.DefaultMaxTimeLossGPS)
            {
                return GetIconCarFromStates(carInfo.IconCode, IconColor.WARNING);
            }

            /* Vehicle time mất > 5 phút < 150 phút -> mất GPS */
            if (StateVehicleExtension.IsLostGPSIcon(carInfo.GPSTime, carInfo.VehicleTime))
            {
                return "ic_lost_gps.png";
            }

            /* Tắt máy */
            // if (isEngineOff() || velocity < getLogin.companyConfig.defaultMaxVelocityGray) {
            if (StateVehicleExtension.IsStopAndEngineOff(carInfo))
            {
                return GetIconCarFromStates(carInfo.IconCode, IconColor.GREY);
            }
            else
            {
                if (StateVehicleExtension.IsMovingAndEngineON(carInfo))
                {
                    // Bật máy
                    if (carInfo.Velocity > CompanyConfigurationHelper.DefaultMaxVelocityOrange)
                    {
                        return GetIconCarFromStates(carInfo.IconCode, IconColor.RED);
                    }
                    else if (carInfo.Velocity > CompanyConfigurationHelper.DefaultMaxVelocityBlue)
                    {
                        return GetIconCarFromStates(carInfo.IconCode, IconColor.ORANGE);
                    }
                    return GetIconCarFromStates(carInfo.IconCode, IconColor.BLUE);
                }
                else
                {
                    return GetIconCarFromStates(carInfo.IconCode, IconColor.BLUE);
                }
            }
        }
    }
}