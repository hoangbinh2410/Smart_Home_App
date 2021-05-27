using System;

namespace BA_MobileGPS.Entities
{
    public class SaveVideoByUserRequest
    {
        public int FK_CompanyID { get; set; }

        public long FK_VehicleID { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsSave { get; set; }

        public string Thumbnail { get; set; }

        public string VideoName { get; set; }

        public byte Channel { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid CreatedUser { get; set; }
    }
}