namespace BA_MobileGPS.Entities
{
    public class MenuItemRespone
    {
        public int PK_MenuItemID { get; set; }

        public int MenuItemParentID { get; set; }

        public int FK_LanguageTypeID { get; set; }

        public int PermissionViewID { get; set; }

        public int SortOrder { get; set; }

        public string NameByCulture { get; set; }

        public string IconMobile { get; set; }

        public string MenuKey { get; set; }

        public string LanguageCode { get; set; }

        public string GroupName { get; set; }
    }
}