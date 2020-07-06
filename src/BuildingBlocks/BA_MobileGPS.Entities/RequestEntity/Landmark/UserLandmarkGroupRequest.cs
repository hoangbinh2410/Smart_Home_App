using System;

namespace BA_MobileGPS.Entities
{
    public class UserLandmarkGroupRequest
    {
        public Guid FK_UserID { get; set; }

        public int FK_LandmarkGroupID { get; set; }

        public bool IsSystem { get; set; }

        public bool IsDisplayBound { get; set; }

        public bool IsDisplayName { get; set; }

        public bool IsVisible { get; set; }
    }
}