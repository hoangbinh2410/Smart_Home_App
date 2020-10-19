using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Camera_Label_TilePage => Get(MobileResourceNames.Camera_Label_TitlePage, "Camera", "Camera");
        public static string Camera_Label_Chanel => Get(MobileResourceNames.Camera_Label_TitlePage, "Kênh: ", "Chanel");
        public static string Camera_TitleStatus => Get(MobileResourceNames.ReportMachine_TitleStatusMachine, "Chọn trạng thái phí", "Search Status");
        public static string Camera_Label_PlaceHolder_Status => Get(MobileResourceNames.ReportMachine_TitleStatusMachine, "Trạng thái phí", "Search Status");

        public static string CameraImage_Label_TitlelPage => Get(MobileResourceNames.CameraImage_Label_TitlelPage, "Giám sát hình ảnh", "Monitoring Image");

        public static string CameraImage_Label_TitleDetailPage => Get(MobileResourceNames.CameraImage_Label_TitleDetailPage, "Xem hình ảnh", "View Image");
        public static string Camera_Label_Undefined => Get(MobileResourceNames.Camera_Label_Undefined, "Không xác định", "Undefined"); 
        public static string Camera_Label_Error => Get(MobileResourceNames.Camera_Label_Error, "Lỗi kết nối...", "Error was happen...");
        public static string Camera_Title_Camera_Streaming => Get(MobileResourceNames.Camera_Title_Camera_Streaming, "Xem trực tuyến", "Camera Streaming");
    }
}