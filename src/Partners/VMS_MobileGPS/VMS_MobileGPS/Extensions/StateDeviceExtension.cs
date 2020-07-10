using BA_MobileGPS.Utilities.Extensions;
using System.Collections.Generic;
using VMS_MobileGPS.Models;

namespace VMS_MobileGPS.Extensions
{
    public static class StateDeviceExtension
    {
        public static string StateDevice(Dictionary<string, string> state)
        {
            string respone = "Đã kết nối";
            if (state.TryGetValue(StateDeviceNameEnums.SAT.ToDescription(), out string satvalue))
            {
                var statussat = SATStatus(satvalue);
                if (string.IsNullOrEmpty(statussat))
                {
                    if (state.TryGetValue(StateDeviceNameEnums.GSM.ToDescription(), out string gsmvalue))
                    {
                        var statusgsm = GSMStatus(gsmvalue);
                        if (string.IsNullOrEmpty(statusgsm))
                        {
                            if (state.TryGetValue(StateDeviceNameEnums.GPS.ToDescription(), out string gpsvalue))
                            {
                                var statusgps = GPSStatus(gpsvalue);
                                if (string.IsNullOrEmpty(statusgps))
                                {
                                    respone = statusgps;
                                }
                            }
                        }
                        else
                        {
                            respone = statusgsm;
                        }
                    }
                }
                else
                {
                    respone = statussat;
                }
            }
            return respone;
        }

        public static bool IsConnectServer(string state)
        {
            if (state == "KN")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GSMStatus(string state)
        {
            var status = state.Split('-');
            if (status != null && status.Length >= 2)
            {
                var stategsm = status[0];
                var ping = int.Parse(status[1]);
                if (stategsm == "ON")
                {
                    if (ping < 12)
                    {
                        return "Tín hiệu yếu";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "Mất tín hiệu";
                }
            }
            else
            {
                return "Mất tín hiệu";
            }
        }

        public static string GPSStatus(string state)
        {
            if (state == "ER")
            {
                return "Mất sóng GPS";
            }
            else if (state == "CO")
            {
                return "";
            }
            else
            {
                return "Sóng GPS yếu";
            }
        }

        public static string PinStatus(string state)
        {
            if (state == "ER")
            {
                return "Lỗi gps";
            }
            else if (state == "CO")
            {
                return "";
            }
            else
            {
                return "Chưa có tọa độ";
            }
        }

        public static string SATStatus(string state)
        {
            var status = state.Split('-');
            if (status != null && status.Length >= 3)
            {
                var statesat = status[0];
                var modesat = status[1];
                var ping = int.Parse(status[2]);
                if (statesat == "OK" && modesat == "ON" && ping > 0 && ping != 255)
                {
                    if (ping < 4)
                    {
                        return "Sóng vệ tinh yếu";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "Mất sóng vệ tinh";
                }
            }
            else
            {
                return "Mất sóng vệ tinh";
            }
        }
    }
}