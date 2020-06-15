using System;

namespace BA_MobileGPS.Entities
{
    public class LanguageRespone : CommonResponseRealBase
    {
        public int PK_LanguageID { set; get; }

        public string CodeName { set; get; }

        public string Icon { set; get; }

        public string Description { set; get; }

        public Guid FK_UserID { set; get; }

        public string NameSort
        {
            get
            {
                if (!string.IsNullOrEmpty(CodeName))
                {
                    return CodeName[0].ToString().ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}