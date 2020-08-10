using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Feedback_Label_TilePage => Get(MobileResourceNames.Feedback_Label_TilePage, "Ý kiến khách hàng", "Feedback");
        public static string Feedback_Label_SubTilePage => Get(MobileResourceNames.Feedback_Label_SubTilePage, "Lắng nghe để phát triển", "Listen to improving");
        public static string Feedback_Label_Content => Get(MobileResourceNames.Feedback_Label_Content, "Ý kiến khách hàng", "Content");
        public static string Feedback_Button_Send => Get(MobileResourceNames.Feedback_Button_Send, "Gửi ý kiến", "Send");

        public static string Feedback_Message_SendSuccess => Get(MobileResourceNames.Feedback_Message_SendSuccess, "Cảm ơn quý khách đã thực hiện đánh giá để chúng tôi phục vụ bạn tốt hơn", "Send feedback successful");

        public static string Feedback_Message_SendFail => Get(MobileResourceNames.Feedback_Message_SendFail, "Gửi đánh giá chưa thành công", "Proccess was unsuccessful");
        public static string Feedback_Message_MarkRequired => Get(MobileResourceNames.Feedback_Message_MarkRequired, "Bạn phải đánh giá tất cả các tiêu chí", "You must rating all categories");

        public static string Feedback_Message_OverMax => Get(MobileResourceNames.Feedback_Message_OverMax, "Bạn đã đánh giá quá {0} lần trong ngày hôm nay", "You had sended feedback over {0} times in today");

        public static string Feedback_Message_FilterWord => Get(MobileResourceNames.Feedback_Message_FilterWord, "Nội dung không được chứa những từ nhạy cảm", "Content don't contain slang words");

        public static string Feedback_Message_Guider1 => Get(MobileResourceNames.Feedback_Message_Guider1, "Chọn số sao đánh giá cho từng loại của", "Tab the star to rate the categories for");
        public static string Feedback_Message_Guider2 => Get(MobileResourceNames.Feedback_Message_Guider2, "APP GPS", "APP GPS");

        public static string Feedback_Label_Thanks => Get(MobileResourceNames.Feedback_Label_Thanks, "Cảm ơn", "Thanks");

        public static string Feedback_Notice_NoEntry => Get(MobileResourceNames.Feedback_Notice_NoEntry, "Bạn chưa nhập nội dung phản hồi", "You did not enter content");

        public static string Feedback_Notice_SendSuccess => Get(MobileResourceNames.Feedback_Notice_SendSuccess, "Cảm ơn quý khách đã phản hồi", "Send feedback successful");

        public static string Feedback_Notice_SendFail => Get(MobileResourceNames.Feedback_Notice_SendFail, "Gửi phản hồi chưa thành công", "Proccess was unsuccessful");
    }
}