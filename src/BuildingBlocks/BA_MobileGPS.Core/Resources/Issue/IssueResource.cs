using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        public static string Issue_Label_TilePage => Get(MobileResourceNames.Issue_Label_TilePage, "Phản hồi thông tin khách hàng", "Feedback customer information");

        public static string ListIssue_Label_TilePage => Get(MobileResourceNames.ListIssue_Label_TilePage, "Danh sách yêu cầu hỗ trợ", "List of support requests");

        public static string ListIssue_Label_DueDate => Get(MobileResourceNames.ListIssue_Label_DueDate, "Lịch hẹn", "Due date");

        public static string ListIssue_Label_NoProcess => Get(MobileResourceNames.ListIssue_Label_NoProcess, "Chưa xử lý", "No process");

        public static string DetailIssue_Label_TilePage => Get(MobileResourceNames.DetailIssue_Label_TilePage, "Chi tiết phản hồi", "Feedback Details");

        public static string DetailIssue_Label_SubmitSupportRequest => Get(MobileResourceNames.DetailIssue_Label_SubmitSupportRequest, "Gửi yêu cầu hỗ trợ", "Submit a support request");

        public static string DetailIssue_Label_DueDate => Get(MobileResourceNames.DetailIssue_Label_DueDate, "Lịch hẹn hoàn thành", "Completed appointment schedule");

        public static string DetailIssue_Label_ContentIssue => Get(MobileResourceNames.DetailIssue_Label_ContentIssue, "NỘI DUNG PHẢN HỒI", "CONTENT OF FEEDBACK");


        public static string DetailIssue_Label_StepIssue => Get(MobileResourceNames.DetailIssue_Label_StepIssue, "THEO DÕI PHẢN HỒI", "FOLLOW FEEDBACK");
       
    }
}