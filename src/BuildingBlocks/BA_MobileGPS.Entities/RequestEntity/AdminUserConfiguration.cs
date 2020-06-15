using System;

namespace BA_MobileGPS.Entities
{
    public class AdminUserConfiguration
    {
        public Guid FK_UserID { get; set; }

        public byte MapType { get; set; }
        public byte MapZoom { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }
    }
}