using System.ComponentModel;

namespace BA_MobileGPS.Entities
{
    /// <summary>
    /// Nhóm trạng thái của xe
    /// Dùng để chọn lọc trạng thái trên trang danh sách xe
    /// </summary>
    /// Name     Date         Comments
    /// namth  1/26/2019   created
    /// </Modified>
    public enum VehicleStatusGroup
    {
        [Description("Tất cả")]
        All = 0,

        [Description("Di chuyển")]
        Moving = 1,

        [Description("Dừng đỗ")]
        Stoping = 2,

        [Description("Bật máy")]
        EngineOn = 3,

        [Description("Tắt máy")]
        EngineOFF = 4,

        [Description("Quá tốc độ")]
        OverVelocity = 5,

        [Description("Mất gps")]
        LostGPS = 6,

        [Description("Mất gsm tín hiệu")]
        LostGSM = 7,

        [Description("Xe nợ phí")]
        VehicleDebtMoney = 8,

        [Description("Lỗi vệ tinh")]
        SatelliteError = 9,
    }
}