using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    /// <summary>
    /// Resource cho trang đăng kí tư vấn
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  2/22/2019   created
    /// </Modified>
    public partial class MobileResource
    {
        public static string RegisterConsult_Label_TilePage => Get(MobileResourceNames.RegisterConsult_Label_TilePage, "Đăng ký tư vấn", "Register Consult");
        public static string RegisterConsult_Label_DescriptionFirstPage => Get(MobileResourceNames.RegisterConsult_Label_DescriptionFirstPage, "Nếu bạn là công ty vận tải chưa sử dụng thiết bị giám sát hành trình để quản lý. Vui lòng để lại thông tin cho chúng tôi để được tư vấn.", "If you are a transport company that has not used cruise monitoring equipment to manage.Please contact us for advice.");
        public static string RegisterConsult_Label_UserName => Get(MobileResourceNames.RegisterConsult_Label_UserName, "Họ và tên (*)", "Full name (*)");
        public static string RegisterConsult_Label_Phone => Get(MobileResourceNames.RegisterConsult_Label_Phone, "Số điện thoại (*)", "Phone number (*)");
        public static string RegisterConsult_Label_TransportType => Get(MobileResourceNames.RegisterConsult_Label_TransportType, "Chọn loại hình vận tải", "Choose the type of transport");
        public static string RegisterConsult_Label_Provinces => Get(MobileResourceNames.RegisterConsult_Label_Provinces, "Chọn tỉnh thành", "Choose a city");
        public static string RegisterConsult_Label_Content => Get(MobileResourceNames.RegisterConsult_Label_Content, "Nhập nội dung cần tư vấn", "Enter the content to be consulted");
        public static string RegisterConsult_Button_Register => Get(MobileResourceNames.RegisterConsult_Button_Register, "Đăng ký", "Register");
        public static string RegisterConsult_Message_Validate_IsNull_UserName => Get(MobileResourceNames.RegisterConsult_Message_Validate_IsNull_UserName, "Họ tên không được để trống", "Full name is empty");
        public static string RegisterConsult_Message_Validate_IsNull_Phone => Get(MobileResourceNames.RegisterConsult_Message_Validate_IsNull_Phone, "Số điện thoại không được để trống", "Phone number is empty");

        public static string RegisterConsult_Message_Validate_MaxLength_UserName => Get(MobileResourceNames.RegisterConsult_Message_Validate_MaxLength_UserName, "Họ tên không được quá {0} kí tự", "Full name must not exceed {0} characters");
        public static string RegisterConsult_Message_Validate_Rule_Phone => Get(MobileResourceNames.RegisterConsult_Message_Validate_Rule_Phone, "Số điện thoại không đúng dịnh dạng", "Phone number is incorrect");

        public static string RegisterConsult_Message_Validate_Rule_Double_Phone => Get(MobileResourceNames.RegisterConsult_Message_Validate_Rule_Double_Phone, "Số điện thoại không được trùng", "Phone number is incorrect");

        public static string RegisterConsult_Message_Validate_IsNull_CountryCode => Get(MobileResourceNames.RegisterConsult_Message_Validate_IsNull_CountryCode, "Mã quốc gia không được để trống", "CountryCode is empty");
        public static string RegisterConsult_Message_ErrorRegister => Get(MobileResourceNames.RegisterConsult_Message_ErrorRegister, "Thao tác thất bại", "Error Register");
        public static string RegisterConsult_Message_ErrorExistsPhone => Get(MobileResourceNames.RegisterConsult_Message_ErrorExistsPhone, "Số điện thoại đã tồn tại", "Phone number is exists");
        public static string RegisterConsult_Message_SuccessRegister => Get(MobileResourceNames.RegisterConsult_Message_SuccessRegister, "✔  Đăng ký của quý khách đã được gửi đến BA thành công. \n ✔  Chúng tôi sẽ liên hệ quý khách trong vòng 1h làm việc. \n ✔  Cảm ơn quý khách hàng đã tin tưởng và sử dụng BA GPS", "✔  Your registration has been successfully sent to BA. \n ✔  We will contact you within 1 working days. \n ✔  Thank you for your trust and use of BA GPS");
        public static string RegisterConsult_Label_TitlePopup => Get(MobileResourceNames.RegisterConsult_Label_TitlePopup, "BA GPS", "BA GPS");
        public static string RegisterConsult_Button_ClosePopup => Get(MobileResourceNames.RegisterConsult_Button_ClosePopup, "Đóng", "Close");
    }
}