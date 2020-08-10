using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string ChangePassword_Label_Title => Get(MobileResourceNames.ChangePassword_Label_Title, "Đổi mật khẩu", "Change password");
        public static string ChangePassword_Label_OldPassword => Get(MobileResourceNames.ChangePassword_Label_OldPassword, "Nhập mật khẩu cũ", "Input old password");
        public static string ChangePassword_Label_OldPassword1 => Get(MobileResourceNames.ChangePassword_Label_OldPassword1, "Nhập mật khẩu cũ (*)", "Old password (*)");
        public static string ChangePassword_Label_NewPassword => Get(MobileResourceNames.ChangePassword_Label_NewPassword, "Nhập mật khẩu mới", "Input new password");
        public static string ChangePassword_Label_NewPassword1 => Get(MobileResourceNames.ChangePassword_Label_NewPassword1, "Nhập mật khẩu mới (*)", "Input new password (*)");
        public static string ChangePassword_Label_NewPasswordConfirm => Get(MobileResourceNames.ChangePassword_Label_NewPasswordConfirm, "Nhập lại mật khẩu mới", "Input New password again");
        public static string ChangePassword_Label_NewPasswordConfirm1 => Get(MobileResourceNames.ChangePassword_Label_NewPasswordConfirm1, "Nhập lại mật khẩu mới (*)", "Input new password again (*)");
        public static string ChangePassword_Validate_OldPasswordEmpty => Get(MobileResourceNames.ChangePassword_Validate_OldPasswordEmpty, "Vui lòng nhập mật khẩu cũ", "Please input old password");
        public static string ChangePassword_Validate_NewPasswordEmpty => Get(MobileResourceNames.ChangePassword_Validate_NewPasswordEmpty, "Vui lòng nhập mật khẩu mới", "Please input new password");
        public static string ChangePassword_Validate_NewPasswordMinLenght => Get(MobileResourceNames.ChangePassword_Validate_NewPasswordMinLenght, "Mật khẩu mới phải có ít nhất {0} kí tự", "New password must have at least {0} characters");
        public static string ChangePassword_Validate_NewPasswordConfirmInvalid => Get(MobileResourceNames.ChangePassword_Validate_NewPasswordConfirmInvalid, "Xác nhận mật khẩu mới không khớp", "Confirm new password does not match");
        public static string ChangePassword_Validate_OldPasswordInvalid => Get(MobileResourceNames.ChangePassword_Validate_OldPasswordInvalid, "Mật khẩu cũ không chính xác", "Old password is incorrect");
        public static string ChangePassword_Validate_NewPasswordIsSameOldPassword => Get(MobileResourceNames.ChangePassword_Validate_NewPasswordIsSameOldPassword, "Mật khẩu mới trùng mật khẩu cũ", "New password matches the old password");
        public static string ChangePassword_Validate_NewPasswordIsSameUserName => Get(MobileResourceNames.ChangePassword_Validate_NewPasswordIsSameUserName, "Mật khẩu mới trùng tên người dùng", "New password matches the user name");
        public static string ChangePassword_Button_ChangePassword => Get(MobileResourceNames.ChangePassword_Button_ChangePassword, "Đổi mật khẩu", "Change password");
        public static string ChangePassword_Message_NewPasswordHasSpace => Get(MobileResourceNames.ChangePassword_Validate_NewPasswordIsSameUserName, "Mật khẩu mới có khoảng trắng. Bạn có muốn đổi thành mật khẩu này không?", "New password has spaces. Do you want to change this password?");
        public static string ChangePassword_Message_UpdateSuccess => Get(MobileResourceNames.ChangePassword_Message_UpdateSuccess, "Quý khách đã đổi mật khẩu thành công, hệ thống sẽ đăng xuất tài khoản đã đăng nhập trên các thiết bị. Vui lòng đăng nhập lại.", "Password is changed successfully, this account will be logged out all devices. Please login again.");
    }
}