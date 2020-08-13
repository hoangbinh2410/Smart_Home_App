using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string SendConfig_Label_TurnOnOff_Running => Get(MobileResourceNames.SendConfig_Label_TurnOnOff_Running, "Đang được thực thi", "Running");

        public static string SendConfig_Label_TurnOnOff_Fail => Get(MobileResourceNames.SendConfig_Label_TurnOnOff_Fail, "Thất bại", "Fail");

        public static string SendConfig_Label_TurnOnOff_Success => Get(MobileResourceNames.SendConfig_Label_TurnOnOff_Success, "Thành công", "Success");

        public static string SendConfig_Label_TurnOnOff_NotOnline => Get(MobileResourceNames.SendConfig_Label_TurnOnOff_NotOnline, "Thiết bị không online", "Device not online");

        public static string SendConfig_Label_TurnOnOff_NotSupport => Get(MobileResourceNames.SendConfig_Label_TurnOnOff_NotSupport, "Lệnh này không được hỗ trợ", "This command is not supported");

        public static string RegisterConfig_Label_PhoneNumber1 => Get(MobileResourceNames.RegisterConfig_Label_PhoneNumber1, "Số điện thoại 1", "Phone number 1");

        public static string RegisterConfig_Label_PhoneNumber2 => Get(MobileResourceNames.RegisterConfig_Label_PhoneNumber2, "Số điện thoại 2", "Phone number 2");

        public static string RegisterConfig_Label_PhoneNumber3 => Get(MobileResourceNames.RegisterConfig_Label_PhoneNumber3, "Số điện thoại 3", "Phone number 3");

        public static string Moto_Label_Setting => Get(MobileResourceNames.Moto_Label_Setting, "Thiết lập", "Setting");

        public static string Moto_Label_Infomation => Get(MobileResourceNames.Moto_Label_Infomation, "Thông tin", "Information");

        public static string Moto_Label_Status_Device => Get(MobileResourceNames.Moto_Label_Status_Device, "Trạng thái của thiết bị", "Status of the device");

        public static string Moto_Label_Alter => Get(MobileResourceNames.Moto_Label_Alter, "Thông báo", "Notification");

        public static string Moto_Label_Confirm => Get(MobileResourceNames.Moto_Label_Confirm, "Xác nhận", "Confirm");

        public static string Moto_Message_Alter => Get(MobileResourceNames.Moto_Message_Alter, "Vui lòng liên hệ tổng đài CSKH để cập nhật số điện thoại cho thiết bị", "Please contact customer service to update the phone number for the device");

        public static string Moto_Message_Alter_WarningNumber => Get(MobileResourceNames.Moto_Message_Alter_WarningNumber, "Bạn có muốn cập nhập ngưỡng cảnh báo SMS không?", "Do you want to update the SMS alert threshold?");

        public static string Moto_Message_AllowTurnOnOffEngineViaSMS => Get(MobileResourceNames.Moto_Message_AllowTurnOnOffEngineViaSMS, "Bạn vui lòng bật Cho phép bật tắt động cơ từ xa qua SMS", "You have not turned on / off the engine remotely");

        public static string Moto_Label_Call_Operator => Get(MobileResourceNames.Moto_Label_Call_Operator, "Gọi tổng đài", "Call the operator");

        public static string Moto_Message_Call_Operator => Get(MobileResourceNames.Moto_Message_Call_Operator, "Bạn có muốn gọi cho thiết bị để phát âm thanh không?", "Would you like to call the device to play a sound?");

        public static string Moto_Message_TurnOn_Engine => Get(MobileResourceNames.Moto_Message_TurnOn_Engine, "Bạn muốn bật động cơ xe?", "Do you want to turn on the device?");

        public static string Moto_Message_TurnOff_Engine => Get(MobileResourceNames.Moto_Message_TurnOff_Engine, "Bạn muốn tắt động cơ xe?", "Do you want to turn off the device?");

        public static string Moto_Message_Send_Location => Get(MobileResourceNames.Moto_Message_Send_Location, "Bạn muốn gửi vị trí xe?", "Do you want to send device location?");

        public static string Moto_Message_Config_PhoneNumber => Get(MobileResourceNames.Moto_Message_Config_PhoneNumber, "Bạn chưa cấu hình số điện thoại", "You have not configured your phone number");

        public static string Moto_Label_Battery_Voltage => Get(MobileResourceNames.Moto_Label_Battery_Voltage, "Điện áp Acquy xe", "Moto battery voltage");

        public static string Moto_Label_Machine_Status => Get(MobileResourceNames.Moto_Label_Machine_Status, "Trạng thái máy", "Machine status");

        public static string Moto_Label_Power_Mode_Used => Get(MobileResourceNames.Moto_Label_Power_Mode_Used, "Chế độ nguồn sử dụng", "Power mode used");

        public static string Moto_Label_Time_Connected_To_The_System => Get(MobileResourceNames.Moto_Label_Time_Connected_To_The_System, "Thời gian kết nối vào hệ thống", "Time connected to the system");

        public static string Moto_Label_Time_To_Update_Location => Get(MobileResourceNames.Moto_Label_Time_To_Update_Location, "Thời gian cập nhật vị trí", "Time to update location");

        public static string Moto_Label_Disconnection_Time => Get(MobileResourceNames.Moto_Label_Disconnection_Time, "Thời gian ngắt kết nối", "Disconnection time");

        public static string Moto_Label_Configure_SMS_Alerts => Get(MobileResourceNames.Moto_Label_Configure_SMS_Alerts, "Cấu hình cảnh báo SMS", "Configure SMS alerts");

        public static string Moto_Label_Allows_Remote_Engine => Get(MobileResourceNames.Moto_Label_Allows_Remote_Engine, "Cho phép bật tắt động cơ từ xa qua SMS", "Allows remote engine on and off via SMS");

        public static string Moto_Label_Allows_Phone_Calls => Get(MobileResourceNames.Moto_Label_Allows_Phone_Calls, "Cho phép gọi điện cảnh báo xe di chuyển trái phép", "Allows phone calls warning unauthorized movement");

        public static string Moto_Label_Turn_On_Alarm => Get(MobileResourceNames.Moto_Label_Turn_On_Alarm, "Bật báo động", "Turn on the alarm");

        public static string Moto_Label_Turn_On_Alarm_Note => Get(MobileResourceNames.Moto_Label_Turn_On_Alarm_Note, "(Gửi SMS bật báo động khi xe di chuyển trái phép)", "(Send SMS to turn on the alarm when the car moves illegally)");

        public static string Moto_Label_Turn_Off_Alarm => Get(MobileResourceNames.Moto_Label_Turn_Off_Alarm, "Tắt báo động", "Turn off the alarm");

        public static string Moto_Label_Turn_Off_Alarm_Note => Get(MobileResourceNames.Moto_Label_Turn_Off_Alarm_Note, "(Gửi SMS tắt báo động khi xe di chuyển trái phép)", "(Send SMS to turn off the alarm when the car moves illegally)");

        public static string Moto_Label_Low_Voltage_Warning => Get(MobileResourceNames.Moto_Label_Low_Voltage_Warning, "Ngưỡng cảnh báo điện áp thấp (mV)", "Low voltage warning threshold (mV)");

        public static string Moto_Label_Set_The_Phone => Get(MobileResourceNames.Moto_Label_Set_The_Phone, "Thiết lập số điện thoại nhận cảnh báo", "Set the phone number to receive alerts");

        public static string Moto_Label_Set_Phone_Number => Get(MobileResourceNames.Moto_Label_Set_Phone_Number, "Số điện thoại nhận cảnh báo", "Phone number to receive alerts");

        public static string Moto_Label_Speed => Get(MobileResourceNames.Moto_Label_Speed, "Vận tốc", "Speed");

        public static string Moto_Label_Status => Get(MobileResourceNames.Moto_Label_Status, "Trạng thái", "Status");
    }
}