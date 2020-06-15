using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Favorites_Label_TilePage => Get(MobileResourceNames.Favorites_Label_TilePage, "Thiết lập mục ưa thích", "Favorites menu setting");
        public static string Favorites_Message_SaveSuccess => Get(MobileResourceNames.Favorites_Message_SaveSuccess, "Lưu mục ưa thích thành công", "Save menu favorites was successfuly");
        public static string Favorites_Message_SaveFail => Get(MobileResourceNames.Favorites_Message_SaveFail, "Lưu menu ưa thích thất bại", "Save menu favorites was unsuccessfuly");

        public static string Favorites_Message_RequiredOne => Get(MobileResourceNames.Favorites_Message_RequiredOne, "Bạn phải chọn ít nhất một tính năng ưa thích", "You must choose at least one");
    }
}