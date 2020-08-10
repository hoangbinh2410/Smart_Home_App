using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Helper_Label_TilePage => Get(MobileResourceNames.Helper_Label_TitlePage, "Trợ giúp", "Helper");
        public static string Helper_Label_HeplperOnline => Get(MobileResourceNames.Helper_Label_HeplperOnline, "Hướng dẫn sử dụng giám sát phương tiện", "Instructions for using car Online");

        public static string Helper_Label_Router => Get(MobileResourceNames.Helper_Label_HeplperRouter, "Hướng dẫn sử dụng lộ trình phương tiện", "Instructions for using car routes");
        public static string Helper_Label_Camera => Get(MobileResourceNames.Helper_Label_HeplperCamera, "Hướng dẫn sử dụng Camera", "Instructions for using Camera");
        public static string Helper_Label_listVihicle => Get(MobileResourceNames.Helper_Label_HeplperlistVihicle, "Hướng dẫn sử dụng danh sách phương tiện", "Instructions for using alerts");
        public static string Helper_Label_PourFuel => Get(MobileResourceNames.Helper_Label_PourFuel, "Hướng dẫn sử dụng báo cáo ", "Instructions for using reports");

        public static string Helper_Label_VihcleDebt => Get(MobileResourceNames.Helper_Label_HeplperVihcleDebt, "Hướng dẫn sử dụng danh sách phương tiện nợ phí", "Instructions for using vehicle debt list");
    }
}