using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        #region TextSupport
        public static string SupportClient_Label_TextTouristsSupport => Get(MobileResourceNames.SupportClient_Label_TextTouristsSupport, "Để hỗ trợ nhanh hơn, quý khách vui lòng thực hiện các kiểm tra trước khi gửi phản hồi", "For faster support, please check before sending feedback");
        public static string SupportClient_Label_PerformChecks => Get(MobileResourceNames.SupportClient_Label_PerformChecks, "Thực hiện kiểm tra", "Perform Checks");
        public static string SupportClient_Label_PerformSendSupport => Get(MobileResourceNames.SupportClient_Label_PerformSendSupport, "Gửi hỗ trợ tới bộ phận CSKH", "Send support to customer service dept");
        public static string SupportClient_Label_HandlingInstructions => Get(MobileResourceNames.SupportClient_Label_PerformSendSupport, "Hướng dẫn xử lý", "Handling instructions");
        public static string SupportClient_Label_Title => Get(MobileResourceNames.SupportClient_Label_Title, "Hỗ trợ khách hàng", "Customer support");
        public static string SupportClient_Label_VehicleProcessing => Get(MobileResourceNames.SupportClient_Label_VehicleProcessing, "Phương tiện xử lý: ", "Vehicle processing: ");
        public static string SupportClient_Label_SupportCategory => Get(MobileResourceNames.SupportClient_Label_SupportCategory, "Danh mục hỗ trợ", "Support category");
        public static string SupportClient_Label_ContentNotify1 => Get(MobileResourceNames.SupportClient_Label_ContentNotify1, "Cảm ơn quý khách đã gửi phản hồi cho BA GPS!", "Thank you for your feedback on BA GPS!");
        public static string SupportClient_Label_ContentNotify2 => Get(MobileResourceNames.SupportClient_Label_ContentNotify2, "Yêu cầu của quý khách đã được gửi đến bộ phận chăm sóc khách hàng của BA GPS. Quý khác vui lòng truy cập tính năng", "Your request has been sent to BA GPS customer service department. Please access the feature");
        public static string SupportClient_Label_ContentNotify3 => Get(MobileResourceNames.SupportClient_Label_ContentNotify3, "Tài khoản > Phản hồi khách hàng", "Account-> customer's feedback");
        public static string SupportClient_Label_ContentNotify4 => Get(MobileResourceNames.SupportClient_Label_ContentNotify4, "Để theo dõi phản hồi của CSKH.", "To track the respond of Customer service department.");

        public static string SupportClient_Label_TextSupport => Get(MobileResourceNames.SupportClient_Label_TextSupport, "Quý khách vui lòng chọn mục cần hỗ trợ dưới đây", "Please select the item you need support below");
        public static string SupportClient_Button_Close => Get(MobileResourceNames.SupportClient_Button_Close, "Đóng", "Close");
        public static string SupportClient_Label_CallToHotline => Get(MobileResourceNames.SupportClient_Label_CallToHotline, "Gọi điện tới hotline CSKH ", "Please call the support hotline ");
        public static string SupportClient_Label_ContentChangePlateNumber1 => Get(MobileResourceNames.SupportClient_Label_ContentChangePlateNumber1, "Phương tiện đủ điều kiện cho phép đổi biển.", "Your vehicle is eligible for a license plate exchange.");
        public static string SupportClient_Label_ContentChangePlateNumber2 => Get(MobileResourceNames.SupportClient_Label_ContentChangePlateNumber2, "Qúy khách vui lòng nhập biển số mới vào ô dưới đây và gửi yêu cầu.", "Please enter your new license plate in the box below and submit your request.");
        public static string SupportClient_Label_ContentChangePlateNumber3 => Get(MobileResourceNames.SupportClient_Label_ContentChangePlateNumber3, "Đổi biển sẽ làm thay đổi dữ liệu của phương tiện được chọn, quý khách cần lưu ý trước khi đổi biển.", "Changing the license plate will change the data of the selected vehicle, you need to be aware before changing the license plate.");
        public static string SupportClient_Label_OldPlate => Get(MobileResourceNames.SupportClient_Label_OldPlate, "Biển số cũ: ", "The old license plate: ");
        public static string SupportClient_Label_NewPlate => Get(MobileResourceNames.SupportClient_Label_NewPlate, "Biển số mới: ", "The new license plate: ");
        public static string SupportClient_Label_EnterNewPlate => Get(MobileResourceNames.SupportClient_Label_EnterNewPlate, "Nhập biển kiếm soát mới", "Enter a new license plate");  
        public static string SupportClient_Label_MessageContent => Get(MobileResourceNames.SupportClient_Label_MessageContent, "Quý khách đã thực hiện tất cả các bước theo hướng dẫn, vui lòng gửi phản hồi nếu lỗi chưa được khắc phục!", "You have followed all the steps as  instructions, please give your feedback if the error is not fixed!");
        public static string SupportClient_Label_MessageFullName => Get(MobileResourceNames.SupportClient_Label_MessageFullName, "Họ và tên", "First and last name");
        public static string SupportClient_Entry_MessageEnterFullName => Get(MobileResourceNames.SupportClient_Entry_MessageEnterFullName, "Nhập họ và tên", "Enter your full name");

        public static string SupportClient_Title_SelectVehicle => Get(MobileResourceNames.SupportClient_Title_SelectVehicle, "Chọn phương tiện xử lý", "Select handling vehicle");
        public static string SupportClient_Entry_SelectVehicleSearch => Get(MobileResourceNames.SupportClient_Entry_SelectVehicleSearch, "Tìm kiếm phương tiện", "Vehicle search");
        public static string SupportClient_Label_SelectVehicleContent => Get(MobileResourceNames.SupportClient_Label_SelectVehicleContent, "Quý khách vui lòng chọn phương tiện cần kiểm tra", "Please select the vehicle you need check below");

        public static string SupportClient_Label_textSupportError => Get(MobileResourceNames.SupportClient_Label_textSupportError, "Quý khách vui lòng kiểm tra xe có xảy ra các tình huống sau không và làm theo hướng dẫn", "Please check your vehicle for the following situations and follow the instructions");
        public static string SupportClient_Text_Yes => Get(MobileResourceNames.SupportClient_Text_Yes, "Có", "Yes");
        public static string SupportClient_Text_No => Get(MobileResourceNames.SupportClient_Text_No, "Không", "No");
        public static string SupportClient_Button_Text_Next => Get(MobileResourceNames.SupportClient_Button_Text_Next, "Tiếp tục", "Next");
        public static string SupportClient_Text_Unfinished => Get(MobileResourceNames.SupportClient_Text_Unfinished, "Chưa thực hiện", "Unfinished");
        public static string SupportClient_Text_Accomplished => Get(MobileResourceNames.SupportClient_Text_Accomplished, "Đã thực hiện", "Accomplished");
        public static string SupportClient_Button_Text_EnterSupport => Get(MobileResourceNames.SupportClient_Button_Text_EnterSupport, "Nhập hỗ trợ", "Enter support");

        public static string SupportClient_Label_TextNosignal => Get(MobileResourceNames.SupportClient_Label_TextNosignal, "Mất tín hiệu", "No signal");
        public static string SupportClient_Label_TextChangePlates => Get(MobileResourceNames.SupportClient_Label_TextChangePlates, "Đổi biển", "Changing the license plate");
        public static string SupportClient_Label_TextCameraError => Get(MobileResourceNames.SupportClient_Label_TextCameraError, "Lỗi camera", "Camera error");
        public static string SupportClient_Label_TextPleaseCheckYourVehicle => Get(MobileResourceNames.SupportClient_Label_TextPleaseCheckYourVehicle, "Qúy khách vui lòng kiểm tra xe có xảy ra tình huống sau không và làm theo hướng dẫn", "Please check your vehicle is in one of the following cases and follow the instructions");
        public static string SupportClient_Label_TextSupportFeedbackError => Get(MobileResourceNames.SupportClient_Label_TextSupportFeedbackError, "Qúy khách đã thực hiện tất các bước theo hướng dẫn, vui lòng gửi phản hồi nếu lỗi chưa được khắc phục", "You have done all the steps according to the instructions, please send feedback if the error has not been fixed");
        public static string SupportClient_Label_LbSupportFeedbackName => Get(MobileResourceNames.SupportClient_Label_LbSupportFeedbackName, "Danh mục cần hỗ trợ: ", "Category to support: ");
        public static string SupportClient_Label_LbSupportFeedbackVehicle => Get(MobileResourceNames.SupportClient_Label_LbSupportFeedbackVehicle, "Phương tiện: ", "Vehicle: ");
        public static string SupportClient_Label_LbSupportFeedbackPhoneNumber => Get(MobileResourceNames.SupportClient_Label_LbSupportFeedbackPhoneNumber, "Số điện thoại", "Phone number");
        public static string SupportClient_Entry_MessageEnterPhoneNumber => Get(MobileResourceNames.SupportClient_Entry_MessageEnterPhoneNumber, "Nhập số điện thoại", "Enter your phone number");
        public static string SupportClient_Label_TextSupportFeedbackContent => Get(MobileResourceNames.SupportClient_Label_TextSupportFeedbackContent, "Nội dung phản hồi", "Feedback content");
        public static string SupportClient_Entry_MessageEnterFeedbackContent => Get(MobileResourceNames.SupportClient_Entry_MessageEnterFeedbackContent, "Nhập nội dung phản hồi", "Enter feedback content");
        public static string SupportClient_Button_BtnSupportFeedbackSend => Get(MobileResourceNames.SupportClient_Button_BtnSupportFeedbackSend, "Gửi phản hồi", "Send feedback");

        #endregion


    }
}



