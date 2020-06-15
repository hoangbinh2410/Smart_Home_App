using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
    public enum UnitsOfLength
    {
        Kilometer,
        NauticalMiles
    }

    public class MapConstant
    {
        // bán kính trái đất
        public static int EarthRadius = 6380000;

        // hằng số delta tọa độ: delta= 180/pi*R(bán kính trái đất)
        public static double DeltaCoordinate = 0.0000089805297042448772;
    }

    public static class MapHelper
    {
        private const double _MilesToKilometers = 1.609344;
        private const double _MilesToNautical = 0.8684;

        /// <summary>
        /// Converts degrees to Radians.
        /// </summary>
        /// <returns>Returns a radian from degrees.</returns>
        public static double ToRadian(this double degree) { return (degree * Math.PI / 180.0); }

        /// <summary>
        /// To degress from a radian value.
        /// </summary>
        /// <returns>Returns degrees from radians.</returns>
        public static double ToDegree(this double radian) { return (radian / Math.PI * 180.0); }

        /// <summary>
        /// Calculates the distance between two points of latitude and longitude.
        /// Great Link - http://www.movable-type.co.uk/scripts/latlong.html
        /// </summary>
        /// <param name="coordinate1">First coordinate.</param>
        /// <param name="coordinate2">Second coordinate.</param>
        /// <param name="unitsOfLength">Sets the return value unit of length.</param>
        public static double Distance(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude, UnitsOfLength unitsOfLength)
        {
            if (_OldLongitude == 0 || _OldLatitude == 0 || Longitude == 0 || Latitude == 0) return 0;
            if (_OldLongitude == Longitude && _OldLatitude == Latitude) return 0;

            var theta = _OldLongitude - Longitude;
            var distance = Math.Sin(_OldLatitude.ToRadian()) * Math.Sin(Latitude.ToRadian()) +
                           Math.Cos(_OldLatitude.ToRadian()) * Math.Cos(Latitude.ToRadian()) *
                           Math.Cos(theta.ToRadian());

            distance = Math.Acos(distance).ToDegree() * 60 * 1.1515;

            switch (unitsOfLength)
            {
                case UnitsOfLength.Kilometer: return distance * _MilesToKilometers;
                case UnitsOfLength.NauticalMiles: return distance * _MilesToNautical;
                default: return distance;
            }
        }

        public static double Distance(Coordinate old, Coordinate current)
        {
            return Distance(old.Longitude, old.Latitude, current.Longitude, current.Latitude, UnitsOfLength.Kilometer);
        }

        private const double R = Math.PI * 6371 / 180;//ban kinh trai dat - km
        private const double SquareR = R * R;
        public const int ConvertKmToMeter = 1000;

        /// <summary>
        /// Tinh khoang cach tuong doi giua hai diem tuong doi gan nhau so voi ban kinh trai dat
        /// </summary>
        /// <param name="a"> toa do diem dau </param>
        /// <param name="b"> toa do diem cuoi </param>
        /// <returns></returns>
        public static double DistanceFloor(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            //double x = (b.Lng - a.Lng) * Math.Cos((a.Lat + b.Lat) / 2);
            double x = (Longitude - _OldLongitude);
            double y = (Latitude - _OldLatitude);
            double distance = Math.Sqrt(x * x + y * y) * R;
            return distance;
        }

        public static double DistanceFloor(Coordinate old, Coordinate current)
        {
            return DistanceFloor(old.Longitude, old.Latitude, current.Longitude, current.Latitude);
        }

        public static double SquareDistanceFloor(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            //double x = (b.Lng - a.Lng) * Math.Cos((a.Lat + b.Lat) / 2);
            double x = (Longitude - _OldLongitude);
            double y = (Latitude - _OldLatitude);
            return (x * x + y * y) * SquareR * ConvertKmToMeter * ConvertKmToMeter;
        }

        public static double DistanceInMeter(double _OldLongitude, double _OldLatitude, double Longitude, double Latitude)
        {
            return ConvertKmToMeter * Distance(_OldLongitude, _OldLatitude, Longitude, Latitude, UnitsOfLength.Kilometer);
        }

        /// <summary>
        /// Tính Cosin của đường ABC
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static double Cosin(double a, double b, double c)
        {
            return (Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(a, 2)) / (2 * b * c);
        }

        /// <summary>
        /// check điểm trong hình chữ nhật
        /// chính là hình chữ nhật ngoại tiếp đường tròn
        /// </summary>
        /// <param name="point">điểm cần check</param>
        /// <param name="radius">bán kính</param>
        /// <param name="center">tâm đường tròn</param>
        /// <returns></returns>
        public static bool CheckPointInRectangle(Coordinate point, Coordinate center, double radius)
        {
            // độ rộng vĩ độ
            double deltaLat = radius * MapConstant.DeltaCoordinate;
            // độ rộng vĩ độ
            double deltaLng = deltaLat / (Math.Cos(center.Latitude * Math.PI / 180));
            // check điểm có nằm trong khoảng vĩ độ
            if (point.Latitude > center.Latitude + deltaLat || point.Latitude < center.Latitude - deltaLat)
                return false;
            // check điểm có nằm trong khoảng kinh độ
            if (point.Longitude > center.Longitude + deltaLng || point.Longitude < center.Longitude - deltaLng)
                return false;
            return true;
        }

        /// <summary>
        /// Hàm kiểm tra một điểm có nằm trong một đa giác hay không
        /// theo thuật toán xét xe nằm trên hay dưới đường thẳng của đa giac
        /// </summary>
        /// <param name="point">điểm cần check</param>
        /// <param name="Polygon">list điểm của đa giác</param>
        /// <returns></returns>
        public static bool CheckPointInsidePolygon(Coordinate point, Coordinate[] Polygon)
        {
            int cn = 0;
            for (int i = 0; i < Polygon.Length - 1; i++)
            {
                if (((Polygon[i].Longitude <= point.Longitude) && (Polygon[i + 1].Longitude > point.Longitude))
                 || ((Polygon[i].Longitude > point.Longitude) && (Polygon[i + 1].Longitude <= point.Longitude)))
                {
                    double vt = (point.Longitude - Polygon[i].Longitude) / (Polygon[i + 1].Longitude - Polygon[i].Longitude);
                    if (point.Latitude < Polygon[i].Latitude + vt * (Polygon[i + 1].Latitude - Polygon[i].Latitude))
                        ++cn;
                }
            }
            return ((cn & 1) == 1);
            // 0 if even (out), and 1 if odd (in)
        }

        /// <summary>
        /// XÁc định hướng
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="oldLongitude"></param>
        /// <param name="oldLatitude"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public static byte Bearing(double distance, byte oldBearing, double oldLongitude, double oldLatitude, double longitude, double latitude)
        {
            //If longitude and latitude are not valid, don't change car's direction
            if (longitude == 0 | latitude == 0) return oldBearing;

            //If distance between two cars is too small, retur old Bearing
            if (distance < 0.02)
            {
                return oldBearing;
            }

            byte _Bearing = 0;
            //Calculate new direction
            double DeltaX = latitude - oldLatitude;
            double DeltaY = longitude - oldLongitude;
            double S = Math.Sqrt(Math.Pow(DeltaX, 2) + Math.Pow(DeltaY, 2));
            double G = Math.Acos(DeltaX / S);
            if (DeltaY < 0) G = 2 * Math.PI - G;
            G = Math.Round(4 * G / Math.PI);
            if (G > 7 || G < 0) G = 0;

            try
            { _Bearing = (byte)G; }
            catch
            { _Bearing = 0; }

            return _Bearing;
        }

        public static byte Bearing(double distance, byte oldBearing, Coordinate old, Coordinate now)
        {
            return Bearing(distance, oldBearing, old.Longitude, old.Latitude, now.Longitude, now.Latitude);
        }

        public static IEnumerable<Coordinate> PolylineAlgorithmDecode(this string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException("encodedPoints");

            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32) break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                yield return new Coordinate((float)(Convert.ToDouble(currentLat) / 1E5), (float)(Convert.ToDouble(currentLng) / 1E5));
            }
        }

        public static double CalAngle(Coordinate A, Coordinate B)
        {
            double DeltaX = B.Latitude - A.Latitude;
            double DeltaY = B.Longitude - A.Longitude;
            return Math.Atan2(DeltaY, DeltaX) * (180 / Math.PI);
        }

        public static string PolylineAlgorithmEncode(this IEnumerable<Coordinate> points)
        {
            var str = new StringBuilder();

            var encodeDiff = (Action<int>)(diff =>
            {
                int shifted = diff << 1;
                if (diff < 0)
                    shifted = ~shifted;

                int rem = shifted;

                while (rem >= 0x20)
                {
                    str.Append((char)((0x20 | (rem & 0x1f)) + 63));

                    rem >>= 5;
                }

                str.Append((char)(rem + 63));
            });

            int lastLat = 0;
            int lastLng = 0;

            foreach (var point in points)
            {
                int lat = (int)Math.Round(point.Latitude * 1E5);
                int lng = (int)Math.Round(point.Longitude * 1E5);

                encodeDiff(lat - lastLat);
                encodeDiff(lng - lastLng);

                lastLat = lat;
                lastLng = lng;
            }

            return str.ToString();
        }
    }
}