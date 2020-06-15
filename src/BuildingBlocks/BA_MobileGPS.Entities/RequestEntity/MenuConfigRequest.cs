using System;

namespace BA_MobileGPS.Entities
{
    public class MenuConfigRequest
    {
        public Guid FK_UserID { get; set; }

        public string NameConfig { get; set; }

        public string ListMenus { get; set; }
    }
}