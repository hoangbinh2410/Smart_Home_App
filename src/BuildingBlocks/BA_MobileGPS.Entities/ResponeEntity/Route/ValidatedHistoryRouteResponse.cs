using System;

namespace BA_MobileGPS.Entities
{
    public class ValidateUserConfigGetHistoryRouteResponse
    {
        public bool Success { set; get; }

        public ValidatedHistoryRouteState State { set; get; }

        // số ngày tối đa cho xem lộ trình
        public int TotalDayConfig { set; get; }

        // ngày nhỏ nhất xem lộ trình
        public DateTime? MinDate { set; get; }

        // ngày lớn nhất xem lộ trình
        public DateTime? MaxDate { set; get; }

        // ngày gia hạn
        public DateTime? MCExpried { set; get; }
    }

    public enum ValidatedHistoryRouteState : byte
    {
        None = 0,
        Success = 1,                // thành công
        OverTotalDateMobile = 2,    // Không được xem quá ngày theo cấu hình mobile
        Expired = 3,                // hết hạn bảo lãnh
        OverDateConfig = 4,         // quá hạn chế dữ liệu bảng User_DynamicUserConfigurations
        DateFuture = 5,             // ngày tương lai
        FromDateOverToDate = 6,     // fromTime > toTime
    }

    public class ValidateUserConfigGetHistoryRouteRequest
    {
        public int CompanyId { set; get; }

        public Guid UserId { set; get; }

        public string VehiclePlate { set; get; }

        public DateTime FromDate { set; get; }

        public DateTime ToDate { set; get; }

        public int AppID { set; get; }
    }
}