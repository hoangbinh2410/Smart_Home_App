using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {

        #region KpiDriverChartPage
        public static string Title_Kpi_Page => Get(MobileResourceNames.Title_Kpi_Page, "Điểm xếp hạng lái xe", "Driving Rating Score");
        public static string Text_Assess => Get(MobileResourceNames.Text_Assess, "Đánh giá tiêu chí lái xe", "Evaluation of driving criteria");
        public static string Title_Tabview => Get(MobileResourceNames.Title_Tabview, "AN TOÀN", "SAFE");
        public static string Before_Day_lb => Get(MobileResourceNames.Before_Day_lb, "Ngày trước", "Befor day");
        public static string Next_Day_lb => Get(MobileResourceNames.Next_Day_lb, "Ngày sau", "Next day");
        public static string Sum_Point_lb => Get(MobileResourceNames.Sum_Point_lb, "Tổng điểm", "Total score");
        public static string Rank_lb => Get(MobileResourceNames.Rank_lb, "Xếp hạng", "Rating");
        public static string Table_lb => Get(MobileResourceNames.Table_lb, "Bảng tiêu chí an toàn", "Safe criteria Table");
        public static string Criteria_lb => Get(MobileResourceNames.Criteria_lb, "Tiêu chí", "Criteria");
        public static string Point_lb => Get(MobileResourceNames.Point_lb, "Điểm", "Point");
        public static string Rank_lb2 => Get(MobileResourceNames.Rank_lb2, "XH", "Rating");
        public static string Safe_Criteria_lb => Get(MobileResourceNames.Safe_Criteria_lb, "Tiêu chí an toàn", "Safe criteria");
        public static string Economical_Tabview => Get(MobileResourceNames.Economical_Tabview, "TIẾT KIỆM", "ECONOMICAL");
        public static string Table_Criteria_lb => Get(MobileResourceNames.Table_Criteria_lb, "Bảng tiêu chí tiết kiệm", "Economical criteria Table");
        public static string Economical_Criteria_lb => Get(MobileResourceNames.Economical_Criteria_lb, "Tiêu chí tiết kiệm", "Economical criteria");
        #endregion KpiDriverChartPage
        #region RankDriverPage
        public static string My_Point_TabView_Title => Get(MobileResourceNames.My_Point_TabView_Title, "Điểm của tôi", "My Point");
        public static string Comment_Span => Get(MobileResourceNames.Comment_Span, "Nhận xét: ", "Comment: ");
        public static string Date_lb => Get(MobileResourceNames.Date_lb, "Ngày", "Date");
        public static string Month_lb => Get(MobileResourceNames.Month_lb, "Tháng", "Month");
        public static string Ordinal_Number_lb => Get(MobileResourceNames.Ordinal_Number_lb, "STT", "NO");
        public static string Detail_lb => Get(MobileResourceNames.Detail_lb, "Chi tiết", "Detail");
        public static string Rank_Table_Tabview => Get(MobileResourceNames.Rank_Table_Tabview, "Bảng xếp hạng", "Chart");
        public static string SearchVehicle => Get(MobileResourceNames.SearchVehicle, "Tìm kiếm lái xe", "Search driver");
        public static string Month_Point_Medium => Get(MobileResourceNames.Month_Point_Medium, "Điểm trung bình tháng", "Average score of the month");
        public static string Driver_lb => Get(MobileResourceNames.Driver_lb, "Lái xe", "Driver");
        public static string Look_lb => Get(MobileResourceNames.Look_lb, "Xem", "Look");
        #endregion RankDriverPage

        #region RankNotDriverPage
        #endregion RankNotDriverPage

    }
}
