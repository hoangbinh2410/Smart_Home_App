using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Image_Lable_ImageMonitoring  => Get(MobileResourceNames.Image_Lable_ImageMonitoring, "Giám sát hình ảnh", "Image Monitoring");
        public static string Image_Alert_SaveImageSuccess => Get(MobileResourceNames.Image_Alert_SaveImageSuccess, "Lưu hình ảnh thành công", "Successfully Saved");
        public static string Image_Lable_Nearestvehicle => Get(MobileResourceNames.Image_Lable_Nearestvehicle, "Phương tiện xem gần nhất", "Nearest vehicle");

        public static string Image_Lable_Totalnumberimages => Get(MobileResourceNames.Image_Lable_Totalnumberimages, "Tổng số hình ảnh :", "Total number of vehicle images :");

    }
}