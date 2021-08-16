using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Login_Textbox_UserName => Get(MobileResourceNames.Login_Textbox_UserName, "Tên đăng nhập", "Username");

        public static string Login_Textbox_Password => Get(MobileResourceNames.Login_Textbox_Password, "Mật khẩu", "Password");

        public static string Login_Button_Login => Get(MobileResourceNames.Login_Button_Login, "Đăng nhập", "Login");

        public static string Login_Checkbox_Rememberme => Get(MobileResourceNames.Login_Checkbox_Rememberme, "Ghi nhớ mật khẩu", "Remember me");

        public static string Login_Checkbox_Autologin => Get(MobileResourceNames.Login_Checkbox_Autologin, "Ghi nhớ đăng nhập", " Remember login");

        public static string Login_Lable_Forgotpassword => Get(MobileResourceNames.Login_Lable_Forgotpassword, "Quên mật khẩu?", "Forgot password?");

        public static string Login_Lable_RegisterSales => Get(MobileResourceNames.Login_Lable_RegisterSales, "Đăng ký tư vấn", "Register sales");

        public static string Login_Lable_ExperienceBA => Get(MobileResourceNames.Login_Lable_ExperienceBA, "Trải nghiệm BA GPS", "Experience BA");

        public static string Login_Lable_SelectLanguage => Get(MobileResourceNames.Login_Lable_SelectLanguage, "Chọn ngôn ngữ", "Select Language");

        public static string Login_Message_AccountPasswordIncorrect => Get(MobileResourceNames.Login_Message_AccountPasswordIncorrect, "Tài khoản hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại", "Account or password is incorrect.Please check again");
        public static string Login_Message_AccountAllowedSystem => Get(MobileResourceNames.Login_Message_AccountPasswordIncorrect, "Tài khoản của bạn không được phép đăng nhập hệ thống", "Your account is not allowed to log in to the system");
        public static string Login_Message_AccountLocked => Get(MobileResourceNames.Login_Message_AccountLocked, "Tài khoản của bạn đang bị khóa", " Your account was locked");
        public static string Login_ForgotPassword_PopupTitle => Get(MobileResourceNames.Login_ForgotPassword_PopupTitle, "Quên mật khẩu", " Forgot Password");
        public static string Login_ForgotPassword_PopupContent => Get(MobileResourceNames.Login_ForgotPassword_PopupContent, "Để đảm bảo an toàn thông tin, Quý khách vui lòng liên hệ <strong> <font color='{0}'> {1} </font> </strong> để được cấp lại mật khẩu", " To ensure information security, please contact <strong> <font color='{0}'> {1} </font> </strong> for a password reset");
        public static string Login_Popup_Starting_Page => Get(MobileResourceNames.Login_Popup_Starting_Page, "Ra khơi", "Starting Page");

        public static string Login_Popup_Manual => Get(MobileResourceNames.Login_Popup_Manual, "Trợ giúp", "User Manual");
        public static string Login_Popup_Guarantee => Get(MobileResourceNames.Login_Popup_Guarantee, "Thông tin bảo hành", "Guarantee Informaion");
        public static string Login_Popup_RegisterSupport => Get(MobileResourceNames.Login_Popup_RegisterSupport, "Đăng ký tư vấn", "Support Register");

        public static string Login_Popup_Network => Get(MobileResourceNames.Login_Popup_Network, "Mạng lưới", "BA Network");
        public static string Login_Popup_BAGPSExperience => Get(MobileResourceNames.Login_Popup_BAGPSExperience, "Trải nghiệm BA GPS", "BA GPS Experience");

        public static string Login_UserNameProperty_NullOrEmpty => Get(MobileResourceNames.Login_UserNameProperty_NullOrEmpty, "Tên đăng nhập không được để trống", "Username cannot be empty");
        public static string Login_PasswordProperty_NullOrEmpty => Get(MobileResourceNames.Login_UserNameProperty_NullOrEmpty, "Mật khẩu không được để trống", "Password cannot be empty");

        public static string Login_Message_LoginWebOnly => Get(MobileResourceNames.Login_Message_LoginWebOnly, "Tài khoản này chỉ được phép đăng nhập trên Web", "This account is only allowed to login on the Web");
        public static string Login_Message_UpdateVersionNew => Get(MobileResourceNames.Login_Message_UpdateVersionNew, "Cập nhập phiên bản mới", "Update new version");

        public static string Login_Message_UpdateVersionMessage => Get(MobileResourceNames.Login_Message_UpdateVersionMessage, "Cập nhật lên phiên bản mới nhất sẽ có nhiều cải tiến và nâng cấp bảo mật giúp bạn có trải nghiệm tốt hơn, an toàn và tiện dụng hơn.",
            "Updating to the latest version includes many security enhancements and enhancements that make your experience better, safer, and more convenient.");


        public static string Login_Message_PleaseCall=> Get(MobileResourceNames.Login_Message_PleaseCall, "Vui lòng gọi đến số {0} để được hỗ trợ", "Please call {0} for assistance");

    }
}