using System;

namespace BA_MobileGPS.Entities
{
    public class RequestUpdateLanguage
    {
        public Guid FK_UserID { set; get; }
        public int FK_LanguageID { set; get; }
        //api/language/updatelanguagebyuser
    }
}