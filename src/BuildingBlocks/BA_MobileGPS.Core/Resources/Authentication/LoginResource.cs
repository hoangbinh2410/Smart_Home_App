using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Login_Textbox_UserName => GetResourceNotDB(MobileResourceNames.Login_Textbox_UserName, "Tên đăng nhập", "Username");

        public static string Login_Textbox_Password => GetResourceNotDB(MobileResourceNames.Login_Textbox_Password, "Mật khẩu", "Password");

        public static string Login_Button_Login => GetResourceNotDB(MobileResourceNames.Login_Button_Login, "Đăng nhập", "Login");

        public static string Login_Checkbox_Rememberme => GetResourceNotDB(MobileResourceNames.Login_Checkbox_Rememberme, "Ghi nhớ mật khẩu", "Remember me");

        public static string Login_Checkbox_Autologin => GetResourceNotDB(MobileResourceNames.Login_Checkbox_Autologin, "Ghi nhớ đăng nhập", " Remember login");

        public static string Login_Lable_Forgotpassword => GetResourceNotDB(MobileResourceNames.Login_Lable_Forgotpassword, "Quên mật khẩu?", "Forgot password?");

        public static string Login_Lable_RegisterSales => GetResourceNotDB(MobileResourceNames.Login_Lable_RegisterSales, "Đăng ký tư vấn", "Register sales");

        public static string Login_Lable_ExperienceBA => GetResourceNotDB(MobileResourceNames.Login_Lable_ExperienceBA, "Trải nghiệm BA GPS", "Experience BA");

        public static string Login_Lable_SelectLanguage => GetResourceNotDB(MobileResourceNames.Login_Lable_SelectLanguage, "Chọn ngôn ngữ", "Select Language");

        public static string Login_Message_AccountPasswordIncorrect => GetResourceNotDB(MobileResourceNames.Login_Message_AccountPasswordIncorrect, "Tài khoản hoặc mật khẩu không chính xác", "Account or password is incorrect");
        public static string Login_Message_AccountAllowedSystem => GetResourceNotDB(MobileResourceNames.Login_Message_AccountPasswordIncorrect, "Tài khoản của bạn không được phép đăng nhập hệ thống", "Your account is not allowed to log in to the system");
        public static string Login_Message_AccountLocked => GetResourceNotDB(MobileResourceNames.Login_Message_AccountLocked, "Tài khoản của bạn đang bị khóa", " Your account was locked");
        public static string Login_ForgotPassword_PopupTitle => GetResourceNotDB(MobileResourceNames.Login_ForgotPassword_PopupTitle, "Quên mật khẩu", " Forgot Password");
        public static string Login_ForgotPassword_PopupContent => GetResourceNotDB(MobileResourceNames.Login_ForgotPassword_PopupContent, string.Format("Để đảm bảo an toàn thông tin, Quý khách vui lòng liên hệ <strong> <font color={0}> {0} </font> </strong> để được cấp lại mật khẩu", MobileSettingHelper.HotlineGps), string.Format(" To ensure information security, please contact <strong> {0} </strong> for a password reset", MobileSettingHelper.HotlineGps));
        public static string Login_Popup_Starting_Page => GetResourceNotDB(MobileResourceNames.Login_Popup_Starting_Page, "Ra khơi", "Starting Page");

        public static string Login_Popup_Manual => GetResourceNotDB(MobileResourceNames.Login_Popup_Manual, "Trợ giúp", "User Manual");
        public static string Login_Popup_Guarantee => GetResourceNotDB(MobileResourceNames.Login_Popup_Guarantee, "Thông tin bảo hành", "Guarantee Informaion");
        public static string Login_Popup_RegisterSupport => GetResourceNotDB(MobileResourceNames.Login_Popup_RegisterSupport, "Đăng ký tư vấn", "Support Register");

        public static string Login_Popup_Network => GetResourceNotDB(MobileResourceNames.Login_Popup_Network, "Mạng lưới", "BA Network");
        public static string Login_Popup_BAGPSExperience => GetResourceNotDB(MobileResourceNames.Login_Popup_BAGPSExperience, "Trải nghiệm BA GPS", "BA GPS Experience");

        public static string Login_UserNameProperty_NullOrEmpty => GetResourceNotDB(MobileResourceNames.Login_UserNameProperty_NullOrEmpty, "Tên đăng nhập không được để trống", "Username cannot be empty");
        public static string Login_PasswordProperty_NullOrEmpty => GetResourceNotDB(MobileResourceNames.Login_UserNameProperty_NullOrEmpty, "Mật khẩu không được để trống", "Password cannot be empty");
    }
}