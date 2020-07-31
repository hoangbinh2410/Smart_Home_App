using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        public static string Login_Textbox_UserName => Get(MobileResourceNames.Login_Textbox_UserName, "Tên đăng nhập", "Username");

        public static string Login_Textbox_Password => Get(MobileResourceNames.Login_Textbox_Password, "Mật khẩu", "Password");

        public static string Login_Button_Login => Get(MobileResourceNames.Login_Button_Login, "Đăng nhập", "Login");

        public static string Login_Checkbox_Rememberme => Get(MobileResourceNames.Login_Checkbox_Rememberme, "Ghi nhớ mật khẩu", "Remember me");

        public static string Login_Checkbox_Autologin => Get(MobileResourceNames.Login_Checkbox_Autologin, "Ghi nhớ đăng nhập", "Automatically login");

        public static string Login_Lable_Forgotpassword => Get(MobileResourceNames.Login_Lable_Forgotpassword, "Quên mật khẩu?", "Forgot password");

        public static string Login_Lable_RegisterSales => Get(MobileResourceNames.Login_Lable_RegisterSales, "Đăng ký tư vấn", "Register sales");

        public static string Login_Lable_ExperienceBA => Get(MobileResourceNames.Login_Lable_ExperienceBA, "Trải nghiệm BA GPS", "Experience BA");

        public static string Login_Lable_SelectLanguage => Get(MobileResourceNames.Login_Lable_SelectLanguage, "Chọn ngôn ngữ", "Select Language");

        public static string Login_Message_AccountPasswordIncorrect => Get(MobileResourceNames.Login_Message_AccountPasswordIncorrect, "Tài khoản hoặc mật khẩu không chính xác", "Account or password is incorrect");

        public static string Login_Message_AccountLocked => Get(MobileResourceNames.Login_Message_AccountLocked, "Tài khoản của bạn đang bị khóa", " Your account is locked");
        public static string Login_ForgotPassword_PopupTitle => Get(MobileResourceNames.Login_ForgotPassword_PopupTitle, "Quên mật khẩu", " Forgot Password");
        public static string Login_ForgotPassword_PopupContent => Get(MobileResourceNames.Login_ForgotPassword_PopupContent, "Để đảm bảo an toàn thông tin, Quý khách vui lòng liên hệ <strong> 19006464 </strong> để được cấp lại mật khẩu", " To ensure information security, please contact <strong> 19006464 </strong> for a password reset");
        public static string Login_Popup_Starting_Page => Get(MobileResourceNames.Login_Popup_Starting_Page, "Ra khơi", "Starting Page");

        public static string Login_Popup_Manual => Get(MobileResourceNames.Login_Popup_Manual, "Hướng dẫn sử dụng", "User Manual");
        public static string Login_Popup_Guarantee => Get(MobileResourceNames.Login_Popup_Guarantee, "Thông tin bảo hành", "Guarantee Informaion");
        public static string Login_Popup_RegisterSupport => Get(MobileResourceNames.Login_Popup_RegisterSupport, "Đăng kí tư vấn", "Support Register");
        public static string Login_Popup_BAGPSExperience => Get(MobileResourceNames.Login_Popup_BAGPSExperience, "Trải nghiệm BA GPS", "BA GPS Experience");


    }
}