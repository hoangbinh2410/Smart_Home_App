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
        public static string Camera_Label_Connection_Error => Get(MobileResourceNames.Camera_Label_Connection_Error, "Lỗi kết nối,\n vui lòng thử lại!", "Connection error,\nplease re-connect");
        public static string Camera_Label_Timeout_Error => Get(MobileResourceNames.Camera_Label_Timeout_Error, "Thời lượng đã hết,\n vui lòng gia hạn!", "Out of time, \nplease re-connect");
        public static string Camera_Title_Camera_Streaming => Get(MobileResourceNames.Camera_Title_Camera_Streaming, "Xem trực tuyến", "Camera Streaming");
        public static string Camera_Title_Retreaming => Get(MobileResourceNames.Camera_Title_Retreaming, "Xem lại video", "Camera ReStream");
        public static string Camera_Label_Time_Has_Video => Get(MobileResourceNames.Camera_Label_Time_Has_Video, "Thời gian có video", "The time has video");
        public static string Camera_Label_Time_Has_VideoSaved => Get(MobileResourceNames.Camera_Label_Time_Has_VideoSaved, "Thời gian video đã lưu", "The time video saved");
        public static string Camera_Label_Vehicle_Numbers => Get(MobileResourceNames.Camera_Label_Vehicle_Numbers, "Tổng số xe", "Total vehicle");
        public static string Camera_Label_Vehicle_Have_Video_Numbers => Get(MobileResourceNames.Camera_Label_Vehicle_Have_Video_Numbers, "Số xe có video", "Total vehicle have video");
        public static string Camera_Label_Video_Numbers => Get(MobileResourceNames.Camera_Title_Retreaming, "Tổng số video", "Total video");
        public static string Camera_Label_Video_Event_Numbers => Get(MobileResourceNames.Camera_Label_Video_Event_Numbers, "Số video sự kiện", "Total video have event");
        public static string Camera_Label_Start_Watch => Get(MobileResourceNames.Camera_Label_Start_Watch, "Xem video", "Start Watch");
        public static string Camera_ChildTab_Device => Get(MobileResourceNames.Camera_ChildTab_Device, "Thiết bị", "Device");
        public static string Camera_ChildTab_BACloud => Get(MobileResourceNames.Camera_ChildTab_BACloud, "BACloud", "BACloud");
        public static string Camera_ChildTab_MyVideo => Get(MobileResourceNames.Camera_ChildTab_MyVideo, "Video của tôi", "My Video");

    }
}