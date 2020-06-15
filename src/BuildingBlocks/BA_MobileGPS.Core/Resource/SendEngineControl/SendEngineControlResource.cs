using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string SendEngineControl_Label_TilePage => Get(MobileResourceNames.SendEngineControl_Label_TilePage, "Tắt bật máy từ xa", "Send Engine Control");

        public static string SendEngineControl_Label_Search => Get(MobileResourceNames.SendEngineControl_Label_Search, "Tìm kiếm", "Search");

        public static string SendEngineControl_Label_On => Get(MobileResourceNames.SendEngineControl_Label_On, "Bật máy", "On Engine");

        public static string SendEngineControl_Label_Off => Get(MobileResourceNames.SendEngineControl_Label_Off, "Tắt máy", "Off Engine");

        public static string SendEngineControl_Table_Serial => Get(MobileResourceNames.SendEngineControl_Table_Serial, "STT", "Serial");

        public static string SendEngineControl_Table_VehiclePlate => Get(MobileResourceNames.SendEngineControl_Table_VehiclePlate, "Phương tiện", "Vehicle Plate");

        public static string SendEngineControl_Table_CurrentTime => Get(MobileResourceNames.SendEngineControl_Table_CurrentTime, "Thời điểm", "Date");

        public static string SendEngineControl_Table_Action => Get(MobileResourceNames.SendEngineControl_Table_Action, "Hành động", "Action");

        public static string SendEngineControl_Table_Status => Get(MobileResourceNames.SendEngineControl_Table_Status, "Trạng thái", "Status");

        public static string SendEngineControl_Popup_Label_Password => Get(MobileResourceNames.SendEngineControl_Popup_Label_Password, "Chức năng này có thể nguy hiểm đến người điều khiển phương tiện. Vui lòng nhập mật khẩu đặng nhập để thực hiện tính năng này", "This function may be dangerous to the driver of the vehicle. Please enter the login password to perform this feature");

        public static string SendEngineControl_Popup_Placeholder_Password => Get(MobileResourceNames.SendEngineControl_Popup_Placeholder_Password, "Nhập mật khẩu", "Enter password");

        public static string SendEngineControl_Button_Password => Get(MobileResourceNames.SendEngineControl_Button_Password, "Thực hiện", "Perform");

        public static string SendEngineControl_Label_Warning => Get(MobileResourceNames.SendEngineControl_Label_Warning, "Xe đang di chuyển bạn không nên thực hiện tắt máy", "The vehicle is moving you should not perform shutdown");

        public static string SendEngineControl_Button_Warning => Get(MobileResourceNames.SendEngineControl_Button_Warning, "Đồng ý", "Agree");

        public static string SendEngineControl_Button_Close => Get(MobileResourceNames.SendEngineControl_Button_Close, "Đóng", "Close");

        public static string SendEngineControl_Popup_HeaderTitle => Get(MobileResourceNames.SendEngineControl_Popup_HeaderTitle, "BA GPS thông báo", "BA GPS Warning");

        public static string SendEngineControl_Warning_Off => Get(MobileResourceNames.SendEngineControl_Warning_Off, "Bạn phải chọn phương tiện để thực hiện tắt máy", "You must choose a means to turn off the device");
       
        public static string SendEngineControl_Warning_On => Get(MobileResourceNames.SendEngineControl_Warning_On, "Bạn phải chọn phương tiện để thực hiện bật máy", "You must choose a means to turn on the device");

        public static string SendEngineControl_Label_State_0 => Get(MobileResourceNames.SendEngineControl_Label_State_0, "Thành công", "Success");

        public static string SendEngineControl_Label_State_1 => Get(MobileResourceNames.SendEngineControl_Label_State_1, "Sai key", "Wrong key");

        public static string SendEngineControl_Label_State_2 => Get(MobileResourceNames.SendEngineControl_Label_State_2, "Thiết bị không online", "Device not online");

        public static string SendEngineControl_Label_State_3 => Get(MobileResourceNames.SendEngineControl_Label_State_3, "Dữ liệu đầu vào không hợp lệ", "Input data is invalid");

        public static string SendEngineControl_Label_State_100 => Get(MobileResourceNames.SendEngineControl_Label_State_100, "Lỗi không xác định", "An unknown error");

        public static string SendEngineControl_Label_On_Success => Get(MobileResourceNames.SendEngineControl_Label_On_Success, "Bật máy thành công", "Turn on success");

        public static string SendEngineControl_Label_Off_Success => Get(MobileResourceNames.SendEngineControl_Label_Off_Success, "Tắt máy thành công", "Turn off success");

        public static string SendEngineControl_Validate_PasswordInvalid => Get(MobileResourceNames.SendEngineControl_Validate_PasswordInvalid, "Mật khẩu không chính xác", "Password is incorrect");

        public static string SendEngineControl_Warning_VehilceNotOnline => Get(MobileResourceNames.SendEngineControl_Warning_VehilceNotOnline, "Xe không online", "Vehilce not online");

        public static string SendEngineControl_Warning_Label => Get(MobileResourceNames.SendEngineControl_Warning_Label, "Cảnh báo", "Warning");

        public static string SendEngineControl_Label_Confirm => Get(MobileResourceNames.SendEngineControl_Label_Confirm, "Xác nhận", "Confirm");

        public static string SendEngineControl_Message_TurnOn_Engine => Get(MobileResourceNames.SendEngineControl_Message_TurnOn_Engine, "Bạn muốn bật máy từ xa?", "Do you want to turn on the device?");

        public static string SendEngineControl_Message_TurnOff_Engine => Get(MobileResourceNames.SendEngineControl_Message_TurnOff_Engine, "Bạn muốn tắt máy từ xa?", "Do you want to turn off the device?");
    }
}