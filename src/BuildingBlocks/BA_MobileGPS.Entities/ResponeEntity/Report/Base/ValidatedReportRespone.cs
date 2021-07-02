namespace BA_MobileGPS.Entities
{
    // trạng thái xử lý validate lấy lộ trình
    public enum StateValidateReport : byte
    {
        None = 0,
        Success = 1, // thành công
        OverDateConfig = 4, // quá hạn chế dữ liệu bảng User_DynamicUserConfigurations
        DateFuture = 5, // ngày tương lai
        FromDateOverToDate = 6, // fromTime > ToTime
    }

    public class ValidatedReportRespone
    {
        public string Message { set; get; }

        public StateValidateReport State { set; get; }
    }
}