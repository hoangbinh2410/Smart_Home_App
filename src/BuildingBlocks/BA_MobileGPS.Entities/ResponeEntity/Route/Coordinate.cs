using System;

namespace BA_MobileGPS.Entities
{
    public class Coordinate : IComparable<Coordinate>
    {
        /// <summary>
        /// Convert một str ra tọa độ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static implicit operator Coordinate(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            //
            var latlng = str.Split(' ');

            // Không thỏa mãn định dạng tọa độ thì thoát
            if (latlng.Length != 2) return null;

            //
            return new Coordinate(Convert.ToSingle(latlng[0]), Convert.ToSingle(latlng[1]));
        }

        public float Longitude { get; set; }

        public float Latitude { get; set; }

        public Coordinate()
        {
        }

        /// <summary>
        /// khởi tạo điểm
        /// </summary>
        /// <param name="lng">kinh độ</param>
        /// <param name="lat">vĩ độ</param>
        public Coordinate(float lat, float lng)
        {
            Longitude = lng;
            Latitude = lat;
        }

        public Coordinate(Coordinate other)
        {
            Longitude = other.Longitude;
            Latitude = other.Latitude;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Latitude + "," + Longitude;
        }

        public string ToPhu()
        {
            return Latitude + " " + Longitude;
        }

        public int CompareTo(Coordinate other)
        {
            if (this.Latitude > other.Latitude) return 1;
            if (this.Latitude < other.Latitude) return -1;
            else return 0;
        }

        /// <summary>
        /// Kinh độ và vĩ độ có thỏa mãn hay không
        /// </summary>
        public bool IsEmpty
        {
            get { return Longitude == 0 || Latitude == 0; }
        }

        /// <summary>
        /// Tính khoảng cách giữa 2 điểm
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static double operator -(Coordinate c1, Coordinate c2)
        {
            if (c1 == null || c2 == null) return 0;
            return MapHelper.Distance(c1.Longitude, c1.Latitude, c2.Longitude, c2.Latitude, UnitsOfLength.Kilometer);
        }

        public Coordinate(string field)
        {
            string[] geo = field.Split(' ');
            Latitude = float.Parse(geo[1]);
            Longitude = float.Parse(geo[0]);
        }

        public bool Equals(Coordinate other)
        {
            if (Math.Abs(this.Latitude - other.Latitude) < 0.000000001 && Math.Abs(this.Longitude - other.Longitude) < 0.000000001)
                return true;
            return false;
        }
    }
}