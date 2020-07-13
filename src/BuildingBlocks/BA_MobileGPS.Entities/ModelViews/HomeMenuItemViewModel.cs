namespace BA_MobileGPS.Entities
{
    public class HomeMenuItemViewModel : BaseModel
    {
        public int PK_MenuItemID { get; set; }

        public int MenuItemParentID { get; set; }

        public int FK_LanguageTypeID { get; set; }

        public int PermissionViewID { get; set; }

        public int SortOrder { get; set; }

        public string NameByCulture { get; set; }

        public string IconMobile { get; set; }

        public string IconMore { get; set; }

        public string MenuKey { get; set; }

        public string LanguageCode { get; set; }

        private bool isVisible = true;

        public bool IsVisible
        { get => isVisible; set => SetProperty(ref isVisible, value, nameof(IsVisible)); }

        private bool isFavorited = false;

        public bool IsFavorited
        { get => isFavorited; set => SetProperty(ref isFavorited, value, nameof(IsFavorited)); }

        public string GroupName
        { get => groupName; set => SetProperty(ref groupName, value, nameof(GroupName)); }

        private string groupName;

    }

    public class HomeMenuItem
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

        public bool IsVisible { get; set; }

        public bool IsFavorited { get; set; }

        public string GroupName { get; set; }
    }
}