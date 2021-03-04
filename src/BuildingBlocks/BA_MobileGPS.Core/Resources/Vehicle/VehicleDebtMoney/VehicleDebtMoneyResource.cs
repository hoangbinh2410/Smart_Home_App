using BA_MobileGPS.Utilities;

namespace BA_MobileGPS.Core.Resources
{
    /// <summary>
    /// Resource cho trang danh sách xe nợ phí
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  2/22/2019   created
    /// </Modified>
    public partial class MobileResource
    {
        public static string VehicleDebtMoney_Label_TilePage => Get(MobileResourceNames.VehicleDebtMoney_Label_TilePage, "Tra cứu thông tin phí", "Search fee information");
        public static string VehicleDebtMoney_Label_SearchFee => Get(MobileResourceNames.VehicleDebtMoney_Label_SearchFee, "Tra cứu phí", "Search fee");
        public static string VehicleDebtMoney_Label_VehicleDebt => Get(MobileResourceNames.VehicleDebtMoney_Label_VehicleDebt, "Phương tiện nợ phí", "Vehicle expired");
        public static string VehicleDebtMoney_Label_Header1 => Get(MobileResourceNames.VehicleDebtMoney_Label_Header1, "Quý khách vui lòng thanh toán phí dịch vụ để việc theo dõi phương tiện không bị gián đoạn", "Please pay the following service fees");
        public static string VehicleDebtMoney_Label_Header2 => Get(MobileResourceNames.VehicleDebtMoney_Label_Header2, "Sau ngày gia hạn quý khách chưa đóng phí thì 1 số tính năng trên hệ thống sẽ bị tạm dừng", "After the renewal date you have not paid the fee, some features on the system will be suspended");
        public static string VehicleDebtMoney_Label_ContactFee => Get(MobileResourceNames.VehicleDebtMoney_Label_ContactFee, "Liên hệ: ", "Contact: ");
        public static string VehicleDebtMoney_Label_PlaceholderSearchKey => Get(MobileResourceNames.VehicleDebtMoney_Label_PlaceholderSearchKey, "Nhập vào biển số phương tiện", "Enter the license plate");
        public static string VehicleDebtMoney_Label_TotalVehicle => Get(MobileResourceNames.VehicleDebtMoney_Label_TotalVehicle, "Tổng số phương tiện: ", "Total number of vehicles");
        public static string VehicleDebtMoney_Label_TitleGrid_ExpireDate => Get(MobileResourceNames.VehicleDebtMoney_Label_TitleGrid_ExpireDate, "Ngày đến hạn", "Date of maturity");
        public static string VehicleDebtMoney_Label_TitleGrid_RenewedDate => Get(MobileResourceNames.VehicleDebtMoney_Label_TitleGrid_RenewedDate, "Gia hạn đến ngày", "Renew to date");
        public static string VehicleDebtMoney_Label_TitleGrid_CountExpireDate => Get(MobileResourceNames.VehicleDebtMoney_Label_TitleGrid_CountExpireDate, "Số ngày quá hạn", "Number of days overdue");
        public static string VehicleDebtMoney_Label_TitleGrid_Descriptions => Get(MobileResourceNames.VehicleDebtMoney_Label_TitleGrid_Descriptions, "Ghi chú", "Note");
        public static string VehicleDebtMoney_TitleStatus => Get(MobileResourceNames.VehicleDebtMoney_TitleStatus, "Chọn trạng thái phí", "Select charge status");
        public static string VehicleDebtMoney_Label_Error_SearchCommand => Get(MobileResourceNames.VehicleDebtMoney_Label_Error_SearchCommand, "Có lỗi xảy ra. Mong quý khách quay lại sau.", "An error occurred. Hope you come back later.");
        public static string VehicleDebtMoney_Label_NoValue_SearchCommand => Get(MobileResourceNames.VehicleDebtMoney_Label_NoValue_SearchCommand, "Không có bản ghi nào phù hợp.", "No records match.");

        public static string VehicleDebtMoney_Message_ID2 => Get(MobileResourceNames.VehicleDebtMoney_Message_ID2, "Phương tiện quá hạn sử dụng", "Vehicle expired");

        public static string VehicleDebtMoney_Message_ID3 => Get(MobileResourceNames.VehicleDebtMoney_Message_ID3, "Phương tiện khóa chưa đóng phí", "The car is locked without charge");

        public static string VehicleDebtMoney_Message_ID128 => Get(MobileResourceNames.VehicleDebtMoney_Message_ID128, "Phương tiện dừng dịch vụ", "Car is under guarantee");

        public static string VehicleDebtMoney_Message_ID0 => Get(MobileResourceNames.VehicleDebtMoney_Message_ID0, "Phương tiện đang hoạt động", "Car is in operation");
        public static string VehicleDebtMoney_Message_ID1 => Get(MobileResourceNames.VehicleDebtMoney_Message_ID1, "Phương tiện sắp đến hạn thu phí", "The vehicle is about to charge");

        public static string VehicleDebtMoneyMessage_SuccessRegister => Get(MobileResourceNames.VehicleDebtMoneyMessage_SuccessRegister, "<span style=\"color:#0984e3\"> ✔  Phương tiện quá hạn sử dung: Chỉ được xem giám sát phương tiện.</span>  <br> <span style=\"color:#ff7675\"> ✔  Phương tiện khóa chưa đóng phí: Không được xem chức năng nào trên hệ thống.</span>. <br> <span style=\"color:#6c5ce7\">✔  Phương tiện dừng dịch vụ: Phương tiện không được xem tính năng nào trên hệ thống và không truyền dữ liệu trên TCĐB </span>.", "✔  Vehicle overdue: Only watch the vehicle.(Blue) \n ✔  Un-paid lock: No function can be viewed on the system..(Red) \n ✔  Guaranteed vehicles: Only see reports and schedules until the end of the guarantee day (Violet)");
        public static string VehicleDebtMoney_Label_TitlePopup => Get(MobileResourceNames.VehicleDebtMoney_Label_TitlePopup, "BA GPS", "BA GPS");
        public static string VehicleDebtMoney_Button_ClosePopup => Get(MobileResourceNames.VehicleDebtMoney_Button_ClosePopup, "Đóng", "Close");
        public static string VehicleDebtMoney_Contact => Get(MobileResourceNames.VehicleDebtMoney_Contact, "{}Liên Hệ: {Key} - {ItemsCount} Phương tiện ", "{}Contact: {Key} - {ItemsCount} vehicle ");

        public static string VehicleFree_Label_TitleGrid_ExpireDate => Get(MobileResourceNames.VehicleDebtMoney_Label_TitleGrid_ExpireDate, "Ngày đóng phí", "Date of maturity");
        public static string VehicleFree_Label_TitleGrid_CountExpireDate => Get(MobileResourceNames.VehicleDebtMoney_Label_TitleGrid_RenewedDate, "Số ngày đến hạn", "Renew to date");
    }
}