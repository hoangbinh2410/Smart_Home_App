using System;

namespace BA_MobileGPS.Core
{
    public static class CalculateDistanceHelper
    {
        //Đo khoảng cách 2 điểm
        public static double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            if (longitude1.Equals(longitude2) && latitude1.Equals(latitude2))
            {
                return 0;
            }

            double p1X = longitude1 * (Math.PI) / 180;
            double p1Y = latitude1 * (Math.PI) / 180;
            double p2X = longitude2 * (Math.PI) / 180;
            double p2Y = latitude2 * (Math.PI) / 180;

            var kc = p2X - p1X;
            kc = Math.Abs(kc);
            var t = Math.Cos(kc);
            t = t * Math.Cos(p2Y);
            t = t * Math.Cos(p1Y);
            kc = Math.Sin(p1Y);
            kc = kc * Math.Sin(p2Y);
            t = t + kc;
            kc = Math.Acos(t);
            kc = kc * 6378137;
            return kc;
        }

        //Đổi meter sang kilometer
        public static double ConvertDistanceMToKm(double number, int start)
        {
            number = number * 0.001;
            return Math.Round(number, start);
        }

        //Đổi meter sang hải lý quốc tế
        public static double ConvertDistanceMToNmi(double number, int start)
        {
            number = number * 0.00053996;
            return Math.Round(number, start);
        }
    }
}