using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class UserSettingsRequest
    {
        public Guid UserID { get; set; }

        public List<MobileUserSetting> ListSettings { get; set; }

        public Guid ExecutedByUser { get; set; }
    }
}