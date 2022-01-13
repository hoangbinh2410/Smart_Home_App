using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    public partial class MobileResource
    {
        #region TextExpense
        public static string ExpenseCar_Text_Vehicle => Get(MobileResourceNames.ExpenseCar_Text_Vehicle, "Phương tiện", "Vehicle");
        public static string ExpenseCar_Text_HaveOrNoData => Get(MobileResourceNames.ExpenseCar_Text_HaveOrNoData, "Không có dữ liệu trong khoảng thời gian tìm kiếm", "No data available for the search period");
        #endregion

        #region ExpenseManagePage
        public static string ExpenseCar_ManagePage_Title => Get(MobileResourceNames.ExpenseCar_ManagePage_Title, "Quản lý chi phí xe", "Vehicle Expense management");
        public static string ExpenseCar_ManagePage_Lable_ExpensePerDay => Get(MobileResourceNames.ExpenseCar_ManagePage_Lable_ExpensePerDay, "Chi phí theo ngày", "Expense per day");
        public static string ExpenseCar_ManagePage_Lable_ExpenseTotal => Get(MobileResourceNames.ExpenseCar_ManagePage_Lable_ExpenseTotal, "TỔNG CHI PHÍ", "TOTAL EXPENSE");
        #endregion     
        #region ExpenseDetailsPage
        public static string ExpenseCar_DetailsPage_Title => Get(MobileResourceNames.ExpenseCar_DetailsPage_Title, "Chi tiết chi phí", "Detailed expense");
        public static string ExpenseCar_DetailsPage_Lable_ChooseDate => Get(MobileResourceNames.ExpenseCar_DetailsPage_Lable_ChooseDate, "Chọn ngày", "Choose date");
        public static string ExpenseCar_DetailsPage_Lable_TypeOfCost => Get(MobileResourceNames.ExpenseCar_DetailsPage_Lable_TypeOfCost, "Loại chi phí", "Type of cost");
        public static string ExpenseCar_DetailsPage_Lable_AllExpense => Get(MobileResourceNames.ExpenseCar_DetailsPage_Lable_AllExpense, "Tất cả chi phí", "All expense");
        public static string ExpenseCar_DetailsPage_Lable_ExpenseTotal => Get(MobileResourceNames.ExpenseCar_DetailsPage_Lable_ExpenseTotal, "Tổng hợp chi phí", "Total expense");
        public static string ExpenseCar_DetailsPage_Lable_ExtraFee => Get(MobileResourceNames.ExpenseCar_DetailsPage_Lable_ExtraFee, "Thêm chi phí", "Extra fee");
        #endregion
        #region ImportExpensePage
        public static string ExpenseCar_ImportPage_Title => Get(MobileResourceNames.ExpenseCar_ImportPage_Title, "Nhập chi phí", "Enter fee");
        public static string ExpenseCar_ImportPage_Lable_TypeOfCost => Get(MobileResourceNames.ExpenseCar_ImportPage_Lable_TypeOfCost, "Loại chi phí", "Type of cost");
        public static string ExpenseCar_ImportPage_Combobox_SelectExpenseType => Get(MobileResourceNames.ExpenseCar_ImportPage_Combobox_SelectExpenseType, "Chọn loại chi phí", "Select expense type");
        public static string ExpenseCar_ImportPage_Combobox_MoneyFee => Get(MobileResourceNames.ExpenseCar_ImportPage_Combobox_MoneyFee, "Số tiền (VNĐ)", "Money fee");
        public static string ExpenseCar_ImportPage_Lable_Note => Get(MobileResourceNames.ExpenseCar_ImportPage_Lable_Note, "Ghi chú", "Note");
        public static string ExpenseCar_ImportPage_Entry_EnterNote => Get(MobileResourceNames.ExpenseCar_ImportPage_Entry_EnterNote, "Nhập ghi chú", "Enter note");
        public static string ExpenseCar_ImportPage_Lable_AddPhoto => Get(MobileResourceNames.ExpenseCar_ImportPage_Lable_AddPhoto, "Thêm hình ảnh", "Add photo");
        public static string ExpenseCar_ImportPage_Button_Save => Get(MobileResourceNames.ExpenseCar_ImportPage_Button_Save, "Lưu", "Save");
        public static string ExpenseCar_ImportPage_Button_SaveAndContinue => Get(MobileResourceNames.ExpenseCar_ImportPage_Button_SaveAndContinue, "Lưu và tiếp tục", "Save and continue");
        public static string ExpenseCar_ImportPage_Lable_ChooseAddress => Get(MobileResourceNames.ExpenseCar_ImportPage_Lable_ChooseAddress, "Chọn điểm", "Choose address");
        public static string ExpenseCar_ImportPage_Entry_EnterAddress => Get(MobileResourceNames.ExpenseCar_ImportPage_Entry_EnterAddress, "Nhập địa chỉ", "Enter address");
        #endregion

    }
}



