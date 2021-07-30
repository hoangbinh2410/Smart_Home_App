using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Camera_Label_TilePage => Get(MobileResourceNames.Camera_Label_TitlePage, "Camera", "Camera");

        public static string Camera_Label_MenuTitle => Get(MobileResourceNames.Camera_Label_MenuTitle, "HÌNH ẢNH - VIDEO", "IMAGE - VIDEO");
        public static string Camera_Label_Chanel => Get(MobileResourceNames.Camera_Label_Chanel, "Kênh: ", "Chanel");
        public static string Camera_TitleStatus => Get(MobileResourceNames.ReportMachine_TitleStatusMachine, "Chọn trạng thái phí", "Search Status");
        public static string Camera_Label_PlaceHolder_Status => Get(MobileResourceNames.ReportMachine_TitleStatusMachine, "Trạng thái phí", "Search Status");
        public static string CameraImage_Label_TitlelPage => Get(MobileResourceNames.CameraImage_Label_TitlelPage, "Giám sát hình ảnh", "Monitoring Image");
        public static string CameraImage_Label_TitleDetailPage => Get(MobileResourceNames.CameraImage_Label_TitleDetailPage, "Xem hình ảnh", "View Image");
        public static string Camera_Label_Undefined => Get(MobileResourceNames.Camera_Label_Undefined, "Không xác định", "Undefined");
        public static string Camera_Label_Connection_Error => Get(MobileResourceNames.Camera_Label_Connection_Error, "Lỗi kết nối,\n vui lòng thử lại!", "Connection error,\nplease re-connect");
        public static string Camera_Label_Timeout_Error => Get(MobileResourceNames.Camera_Label_Timeout_Error, "Thời lượng đã hết,\n vui lòng gia hạn!", "Out of time, \nplease re-connect");
        public static string Camera_Title_Camera_Streaming => Get(MobileResourceNames.Camera_Title_Camera_Streaming, "Giám sát Camera", "Live streaming video");
        public static string Camera_Title_Retreaming => Get(MobileResourceNames.Camera_Title_Retreaming, "Xem lại video", "Camera ReStream");
        public static string Camera_Label_Time_Has_Video => Get(MobileResourceNames.Camera_Label_Time_Has_Video, "Thời gian có video", "The time has video");
        public static string Camera_Label_Time_Has_VideoSaved => Get(MobileResourceNames.Camera_Label_Time_Has_VideoSaved, "Thời gian video đã lưu", "The time video saved");
        public static string Camera_Label_Vehicle_Numbers => Get(MobileResourceNames.Camera_Label_Vehicle_Numbers, "Tổng số xe", "Total vehicle");
        public static string Camera_Label_Vehicle_Have_Video_Numbers => Get(MobileResourceNames.Camera_Label_Vehicle_Have_Video_Numbers, "Số xe có video", "Total vehicle have video");
        public static string Camera_Label_Video_Numbers => Get(MobileResourceNames.Camera_Title_Retreaming, "Tổng số video", "Total video");
        public static string Camera_Label_Video_Event_Numbers => Get(MobileResourceNames.Camera_Label_Video_Event_Numbers, "Số video sự kiện", "Total video have event");
        public static string Camera_Label_Start_Watch => Get(MobileResourceNames.Camera_Label_Start_Watch, "Xem video", "Start Watch");
        public static string Camera_ChildTab_Device => Get(MobileResourceNames.Camera_ChildTab_Device, "Thiết bị", "Device");
        public static string Camera_ChildTab_BACloud => Get(MobileResourceNames.Camera_ChildTab_BACloud, "Tải về", "Download");
        public static string Camera_Lable_ListVideo => Get(MobileResourceNames.Camera_Lable_ListVideo, "Danh sách video", "List of videos");

        public static string Camera_Alert_Title => Get(MobileResourceNames.Camera_Alert_Title, "Thông báo", "Notification");
        public static string Camera_Alert_HelpContent => Get(MobileResourceNames.Camera_Alert_HelpContent, "Các xe sử dụng gói cước không tích hợp tính năng xem video sẽ không được hiển thị trên tính năng này",
            "The vehicle service plan don't apply to watch Live video streaming");
        public static string Camera_Checkbox_AutoRenewal => Get(MobileResourceNames.Camera_Checkbox_AutoRenewal, "Tự gia hạn", "Auto-renewal");
        public static string Camera_Lable_Channel => Get(MobileResourceNames.Camera_Lable_Channel, "Kênh", "Channel");
        public static string Camera_Lable_AllChannel => Get(MobileResourceNames.Camera_Lable_AllChannel, "Tất cả kênh", "All Channels");
        public static string Camera_Lable_Chart => Get(MobileResourceNames.Camera_Lable_Chart, "Xem lại video - Tổng quan", "Overview chart of  all video data");
        public static string Camera_Lable_TimeHasVideo => Get(MobileResourceNames.Camera_Lable_TimeHasVideo, "Thời gian có video", "Recording videos data");
        public static string Camera_Lable_TimeSaveVideo => Get(MobileResourceNames.Camera_Lable_TimeSaveVideo, "Thời gian video đã lưu", "Download videos data");
        public static string Camera_Lable_NoteChart => Get(MobileResourceNames.Camera_Lable_NoteChart, "Thiết bị không ghi video khi phương tiện tắt máy", "The camera is not recording when turning off vehicle engine");

        public static string Camera_Title_UploadVideoToServer => Get(MobileResourceNames.Camera_Title_UploadVideoToServer, "Tải về server", "Download file from Device");
        public static string Camera_Title_NoteDowLoadFile => Get(MobileResourceNames.Camera_Title_NoteDowLoadFile, "Quý khách đang tải video của xe:", "Downloading file:");

        public static string Camera_Lable_NoteUploadVideoToServer => Get(MobileResourceNames.Camera_Lable_NoteUploadVideoToServer, "Video tải trực tiếp từ thiết bị và được chia nhỏ thành các phần để tối ưu đường truyền và dung lượng lưu trữ. Quý khách có thể chọn phần video muốn tải về.",
            "Videos are downloaded from the device directly. Each file is splited into multiple files to optimize transmission and storage capacity. You can select single or multiple files  to download.");

        public static string UploadVideoTo_Alert_NoteUploaded => Get(MobileResourceNames.UploadVideoTo_Alert_NoteUploaded, "Video đang được tải về server. Quý khách vui lòng xem video đã tải trong tab tải về", "Video is downloading. Please watch on Downloaded Videos tab");

        public static string Camera_Title_Downloadedvideo => Get(MobileResourceNames.Camera_Title_Downloadedvideo, "Video đã tải", "Downloaded video/file");
        public static string Camera_Title_Downloadingvideo => Get(MobileResourceNames.Camera_Title_Downloadingvideo, "Video đang tải", "Downloading video/file");
        public static string Camera_Title_ListDownloadingvideo => Get(MobileResourceNames.Camera_Title_ListDownloadingvideo, "Danh sách video đang tải", "Downloading list video/file");
        public static string Camera_Status_ErrorConnect => Get(MobileResourceNames.Camera_Status_ErrorConnect, "Lỗi kết nối. Vui lòng thử lại", "Your connection is not private. Try again!");
        public static string Camera_Status_Downloaded => Get(MobileResourceNames.Camera_Status_WaitingUpload, "Đã tải", "Downloaded");
        public static string Camera_Status_WaitingUpload => Get(MobileResourceNames.Camera_Status_WaitingUpload, "Chờ tải", "Wait File Download");
        public static string Camera_Status_Downloading => Get(MobileResourceNames.Camera_Status_Downloading, "Đang tải", "Downloading");
        public static string Camera_Status_DownloadError => Get(MobileResourceNames.Camera_Status_DownloadError, "Có lỗi khi tải", "Download failed");

        public static string Camera_Alert_DownloadedVideo => Get(MobileResourceNames.Camera_Alert_DownloadedVideo, "Đã tải xong 1 video. Vui lòng xem trên tab Video đã tải", "Download completed. Please watch on Downloaded Videos tab");
    }
}