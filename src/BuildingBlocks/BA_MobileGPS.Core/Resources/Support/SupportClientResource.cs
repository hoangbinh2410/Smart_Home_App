using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        #region SupportClientPage
        public static string SupportClient_Label_Title => Get(MobileResourceNames.SupportClient_Label_Title, "Hỗ trợ khách hàng", "Customer support");
        public static string SupportClient_Label_VehicleProcessing => Get(MobileResourceNames.SupportClient_Label_VehicleProcessing, "Phương tiện xử lý", "Vehicle processing");
        public static string SupportClient_Label_SupportCategory => Get(MobileResourceNames.SupportClient_Label_SupportCategory, "Danh mục hỗ trợ", "Support category");
        public static string SupportClient_Label_TextSupport => Get(MobileResourceNames.SupportClient_Label_TextSupport, "Quý khách vui lòng chọn mục cần hỗ trợ dưới đây", "Please select the item you need support below");
        public static string SupportClient_Label_ChangeNumberPlate => Get(MobileResourceNames.SupportClient_Label_ChangeNumberPlate, "Đổi biển", "Change number plate");
        public static string SupportClient_Label_CameraError => Get(MobileResourceNames.SupportClient_Label_CameraError, "Lỗi camera", "Camera error");
        public static string SupportClient_Button_Close => Get(MobileResourceNames.SupportClient_Button_Close, "Đóng", "Close");
        #endregion SupportClientPage

        #region SupportErrorsPage
        public static string SupportClient_Label_textSupportError => Get(MobileResourceNames.SupportClient_Label_textSupportError, "Quý khách vui lòng kiểm tra xe có xảy ra các tình huống sau không và làm theo hướng dẫn", "Please check your vehicle for the following situations and follow the instructions");
        public static string SupportClient_Text_Yes => Get(MobileResourceNames.SupportClient_Text_Yes, "Có", "Yes");
        public static string SupportClient_Text_No => Get(MobileResourceNames.SupportClient_Text_No, "Không", "No");
        public static string SupportClient_Text_Unfinished => Get(MobileResourceNames.SupportClient_Text_Unfinished, "Chưa thực hiện", "Unfinished");
        public static string SupportClient_Text_Accomplished => Get(MobileResourceNames.SupportClient_Text_Accomplished, "Đã thực hiện", "Accomplished");
        #endregion SupportErrorsSignalPage

        #region FeedbackErrorsSignalPage
        public static string SupportClient_Label_TextSupportFeedbackCameraError => Get(MobileResourceNames.SupportClient_Label_TextSupportFeedbackCameraError, "Qúy khách đã thực hiện tất các bước theo hướng dẫn, vui lòng gửi phản hồi nếu lỗi chưa được khắc phục", "You have done all the steps according to the instructions, please send feedback if the error has not been fixed");
        public static string SupportClient_Label_LbSupportFeedbackName => Get(MobileResourceNames.SupportClient_Label_LbSupportFeedbackName, "Danh mục cần hỗ trợ: Mất tín hiệu", "Category to support: Loss of signa");
        public static string SupportClient_Label_LbSupportFeedbackVehicle => Get(MobileResourceNames.SupportClient_Label_LbSupportFeedbackVehicle, "Phương tiện: ", "Vehicle: ");
        public static string SupportClient_Label_LbSupportFeedbackPhoneNumber => Get(MobileResourceNames.SupportClient_Label_LbSupportFeedbackPhoneNumber, "Số điện thoại", "Phone number");
        public static string SupportClient_Label_TextSupportFeedbackContent => Get(MobileResourceNames.SupportClient_Label_TextSupportFeedbackContent, "Nội dung phản hồi", "Feedback content");
        public static string SupportClient_Button_BtnSupportFeedbackSend => Get(MobileResourceNames.SupportClient_Button_BtnSupportFeedbackSend, "Gửi phản hồi", "Send feedback");
        #endregion FeedbackErrorsSignalPage


    }
}



