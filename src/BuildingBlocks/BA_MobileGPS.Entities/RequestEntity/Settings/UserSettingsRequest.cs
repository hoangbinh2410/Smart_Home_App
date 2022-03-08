using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Entities
{
    public class UserSettingsRequest
    {
        public Guid UserID { get; set; }

        public List<MobileUserSetting> ListUserSettings { get; set; }

        public Guid ExecutedByUser { get; set; }
    }
}