using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resource
{
    public partial class MobileResource
    {
        #region Forgot Password

        public static string ForgotPassword_Label_TilePage => Get(MobileResourceNames.ForgotPassword_Label_TilePage, "Quên mật khẩu", "Forgot password");
        public static string ForgotPassword_Label_AccountName => Get(MobileResourceNames.ForgotPassword_Label_AccountName, "Nhập tài khoản", "Account");
        public static string ForgotPassword_Label_Phone => Get(MobileResourceNames.ForgotPassword_Label_Phone, "Số điện thoại", "Phone number");
        public static string ForgotPassword_Label_Notes => Get(MobileResourceNames.ForgotPassword_Label_Notes, "Số điện thoại trùng với số điện thoại đăng ký tài khoản", "Phone number is the same as the phone number registered for the account");
        public static string ForgotPassword_Button_SendInfor => Get(MobileResourceNames.ForgotPassword_Button_SendInfor, "Gửi", "Send");
        public static string ForgotPassword_Message_Validate_IsNull_AccountName => Get(MobileResourceNames.ForgotPassword_Message_Validate_IsNull_AccountName, "Tài khoản không được để trống", "Account is empty");
        public static string ForgotPassword_Message_Validate_MaxLength_AccountName => Get(MobileResourceNames.ForgotPassword_Message_Validate_MaxLength_AccountName, "Tài khoản được quá {0} kí tự", "Account must not exceed {0} characters");
        public static string ForgotPassword_Message_Validate_ConstantHTML_AccountName => Get(MobileResourceNames.ForgotPassword_Message_Validate_ConstantHTML_AccountName, "Dữ liệu không được chứa kí tự đặc biệt", "Data cannot contain special characters");
        public static string ForgotPassword_Message_Validate_IsNull_Phone => Get(MobileResourceNames.ForgotPassword_Message_Validate_IsNull_Phone, "Số điện thoại không được để trống", "Phone number is empty");
        public static string ForgotPassword_Message_Validate_Rule_Phone => Get(MobileResourceNames.ForgotPassword_Message_Validate_Rule_Phone, "Số điện thoại không đúng định dạng", "Phone number is incorrect");
        public static string ForgotPassword_Message_ErrorSendSMS => Get(MobileResourceNames.ForgotPassword_Message_ErrorSendSMS, "Lỗi gửi SMS xác thực. Mong quý khách quay lại sau.", "Error sending authentication SMS. ");
        public static string ForgotPassword_Message_SuccessSendSMS => Get(MobileResourceNames.ForgotPassword_Message_SuccessSendSMS, "Gửi mã xác thực thành công", "Success sending authentication SMS. ");
        public static string ForgotPassword_Message_ErrorCheckInforUser => Get(MobileResourceNames.ForgotPassword_Message_ErrorCheckInforUser, "Tài khoản hoặc số điện thoại không tồn tại", "Account or phone number does not exist");
        public static string ForgotPassword_Message_ErrorOverCountOneDay => Get(MobileResourceNames.ForgotPassword_Message_ErrorOverCountOneDay, "Vượt quá số lần gửi trong ngày", "Exceeded the number of times sent in the day");
        public static string ForgotPassword_Message_ErrorWasRegisterSuccess => Get(MobileResourceNames.ForgotPassword_Message_ErrorWasRegisterSuccess, "Đã kích hoạt thành công trước đó", "Successfully activated previously");
        public static string ForgotPassword_Message_ErrorErrorSendSMS => Get(MobileResourceNames.ForgotPassword_Message_ErrorErrorSendSMS, "Lỗi khi gửi thông tin SMS", "Error sending SMS information");
        public static string ForgotPassword_Message_ErrorErrorLogSMS => Get(MobileResourceNames.ForgotPassword_Message_ErrorErrorLogSMS, "Lỗi khi lưu thông tin xác thực", "Error saving credentials");

        #endregion Forgot Password

        #region Verify Code

        public static string VerifyCodeMS_Label_TilePage => Get(MobileResourceNames.VerifyCodeMS_Label_TilePage, "Nhập mã xác thực", "Enter Auth Code");
        public static string VerifyCodeMS_Label_Code => Get(MobileResourceNames.VerifyCodeMS_Label_Code, "Nhập mã xác thực", "Enter the code you rceive via SMS");
        public static string VerifyCodeMS_Label_TimeCountDown => Get(MobileResourceNames.VerifyCodeMS_Label_TimeCountDown, "Mã xác thực có hiệu lực trong vòng: {0}s", "Mã xác thực có hiệu lực trong vòng: {0}s");
        public static string VerifyCodeMS_Button_CheckVerify => Get(MobileResourceNames.VerifyCodeMS_Button_CheckVerify, "Nhập mã xác thực", "Enter Auth Code");
        public static string VerifyCodeMS_Button_ResendCode => Get(MobileResourceNames.VerifyCodeMS_Button_ResendCode, "Gửi lại mã xác thực", "Resend verify code");
        public static string VerifyCodeMS_Message_Validate_IsNull_Code => Get(MobileResourceNames.VerifyCodeMS_Message_Validate_IsNull_Code, "Mã xác thực không được để trống", "Verify Code is empty");
        public static string VerifyCodeMS_Message_Validate_MaxLength_Code => Get(MobileResourceNames.VerifyCodeMS_Message_Validate_MaxLength_Code, "Mã xác thực không được quá {0} kí tự", "Verify Code must not exceed {0} characters");
        public static string VerifyCodeMS_Message_ErrorVerifyCode => Get(MobileResourceNames.VerifyCodeMS_Message_ErrorVerifyCode, "Bị lỗi xác thực", "Error Verify Code.");
        public static string VerifyCodeMS_Message_ErrorWrongVerifyCode => Get(MobileResourceNames.VerifyCodeMS_Message_ErrorWrongVerifyCode, "Nhập sai mã xác thực", "Enter the authentication code wrong");
        public static string VerifyCodeMS_Message_ErrorTimeOut => Get(MobileResourceNames.VerifyCodeMS_Message_ErrorTimeOut, "Quá thời gian nhập mã xác thực", "Time to enter the authentication code.");
        public static string VerifyCodeMS_Message_ErrorOverWrongPerCode => Get(MobileResourceNames.VerifyCodeMS_Message_ErrorOverWrongPerCode, "Quá số lần cho phép nhập sai mã xác thực", "Too many times allowed to enter the authentication code wrong");
        public static string VerifyCodeMS_Message_ErrorOverWrongPerDay => Get(MobileResourceNames.VerifyCodeMS_Message_ErrorOverWrongPerDay, "Sai quá số lần cho phép sai trong ngày", "Wrong number of false allowances in the day");

        #endregion Verify Code

        #region ChangePassForgot

        public static string ChangePassForgot_Label_TilePage => Get(MobileResourceNames.ChangePassForgot_Label_TilePage, "Thay đổi mật khẩu", "Change account password");
        public static string ChangePassForgot_Label_NewPassword => Get(MobileResourceNames.ChangePassForgot_Label_NewPassword, "Nhập mật khẩu mới", "New password");
        public static string ChangePassForgot_Label_ReplyPassword => Get(MobileResourceNames.ChangePassForgot_Label_ReplyPassword, "Nhập lại mật khẩu", "Confirm new password");
        public static string ChangePassForgot_Button_ChangePass => Get(MobileResourceNames.ChangePassForgot_Button_ChangePass, "Đổi mật khẩu", "Change password");
        public static string ChangePassForgot_Message_Validate_IsNull_NewPassword => Get(MobileResourceNames.ChangePassForgot_Message_Validate_IsNull_NewPassword, "Mật khẩu mới không được để trống", "New password is empty");
        public static string ChangePassForgot_Message_Validate_MinLength_NewPassword => Get(MobileResourceNames.ChangePassForgot_Message_Validate_MinLength_NewPassword, "Mật khẩu không được nhỏ hơn {0} kí tự", "New password must not smaller {0} characters");
        public static string ChangePassForgot_Message_Validate_MaxLength_NewPassword => Get(MobileResourceNames.ChangePassForgot_Message_Validate_MaxLength_NewPassword, "Mật khẩu không được quá {0} kí tự", "New password must not exceed {0} characters");
        public static string ChangePassForgot_Message_Validate_IsNull_ReplyPassword => Get(MobileResourceNames.ChangePassForgot_Message_Validate_IsNull_ReplyPassword, "Trường này không được bỏ trống", "Confirm new password is empty");
        public static string ChangePassForgot_Message_Validate_Equal_ReplyPassword => Get(MobileResourceNames.ChangePassForgot_Message_Validate_Equal_ReplyPassword, "Xác nhận mật khẩu mới không khớp", "Confirm new password does not match");
        public static string ChangePassForgot_Message_ErrorChangePassword => Get(MobileResourceNames.ChangePassForgot_Message_ErrorChangePassword, "Bị lỗi thay đổi mật khẩu", "Error change password.");

        public static string ChangePassForgot_Label_TitlePopupSuccessChangePassword => Get(MobileResourceNames.ChangePassForgot_Label_TitlePopupSuccessChangePassword, "BA GPS", "BA GPS");
        public static string ChangePassForgot_Message_SuccessChangePassword => Get(MobileResourceNames.ChangePassForgot_Message_SuccessChangePassword, "Quý khách đã đổi mật khẩu thành công.Vui lòng đăng nhập vào hệ thống.", "Success change password.");
        public static string ChangePassForgot_Button_ClosePopup => Get(MobileResourceNames.ChangePassForgot_Button_ClosePopup, "Đóng", "Close");

        #endregion ChangePassForgot
    }
}