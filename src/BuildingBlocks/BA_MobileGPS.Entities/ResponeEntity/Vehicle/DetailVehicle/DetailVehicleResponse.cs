using Newtonsoft.Json;

using System;

namespace BA_MobileGPS.Entities
{
    public class DetailVehicleResponse
    {
        public InforCommonVehicleResponse itemInforCommon { get; set; }
        public InforBGTResponse itemInforBGT { get; set; }
        public InforChargeMoneyResponse itemInforChargeMoney { get; set; }
        public bool IsShowInforChargeMoney { get; set; }
    }

    /// <summary>
    /// thông tin cơ bản về xe
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  10/03/2019   created
    /// </Modified>
    public class InforCommonVehicleResponse
    {
        public int PK_VehicleID { get; set; }
        public string VehiclePlate { get; set; }
        public string Address { get; set; }
        public string IconVehicle { get; set; }
        public DateTime Time { get; set; }
        public double Speed { get; set; }
        public double Distance { get; set; }
        public int StatusEngineer { get; set; }

        public float KilometInToDay { get; set; }
        public double? KilometAccumulated { get; set; }
        public int? CountStopVehicle { get; set; }

        // đang đỗ
        public int? ParkingVehicleNow { get; set; }

        // dừng đỗ nổ máy
        public int? ParkingTurnOnVehecle { get; set; }

        // điều hoà nhiệt độ
        public bool AirCondition { get; set; }

        public int? CountOpenDoor { get; set; }

        // cẩu
        public bool? Crane { get; set; }

        // ben
        public bool? Ben { get; set; }

        //nhiệt độ
        public string Temperature { get; set; }

        // nhiêu liệu
        public string Fuel { get; set; }

        // bê tông
        public bool? Concrete { get; set; }

        //thẻ nhớ
        public string MemoryStick { get; set; }

        // số lần quá tốc độ
        public int SpeedOverCount { get; set; }

        // Thời gian lái xe liên tục
        public int MinutesOfDrivingTimeContinuous { get; set; }

        // thời gian lái xe trong ngày
        public int MinutesOfDrivingTimeInDay { get; set; }

        /// <summary>
        /// Trạng thái ACC
        /// </summary>
        public bool? AccStatus { set; get; }

        public uint LogfileProcessModules { set; get; }

        public int CancelErrorOptions { set; get; }

        public bool? Door { set; get; }

        public bool? ConcretePump { set; get; }

        public bool IsUseFuel { set; get; }

        /// <summary>
        /// Số điện thoại đăng kí của xe
        /// </summary>
        public string SIMPhoneNumber { set; get; }

        /// <summary>
        /// Số IMEI đăng kí của xe
        /// </summary>
        public string IMEI { set; get; }

        /// <summary>
        /// Ngày đăng ký dịch vụ
        /// </summary>
        public DateTime JoinSystemDate { set; get; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? MCExpried { get; set; }
    }

    /// <summary>
    /// thông tin BGT của xe
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  10/03/2019   created
    /// </Modified>
    public class InforBGTResponse
    {
        public int DriverID { get; set; }
        public string DriverName { get; set; }
        public string DriverLicense { get; set; }
        public string PhoneNumber { get; set; }

        // Sở quản lý
        public string DepartmentOfManagement { get; set; }
    }

    /// <summary>
    /// thông tin phí của xe
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  10/03/2019   created
    /// </Modified>
    public class InforChargeMoneyResponse
    {
        public DateTime ExpireDate { get; set; }
    }

    /// <summary>
    /// thông tin thay đổi theo signalr bắn về - chỉ những thông tin thay đổi mới có trong hàm này
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  10/03/2019   created
    /// </Modified>
    public class InforVehicleChangeBySignalr
    {
        public string Address { get; set; }
        public DateTime Time { get; set; }
        public double Speed { get; set; }
        public double Distance { get; set; }
    }

    public class VehicleOnlineDetailViewModel
    {
        [JsonProperty("0")]
        public VehicleHtn VehicleHtn { set; get; }

        [JsonProperty("1")]
        public VehicleNl VehicleNl { set; get; }

        [JsonProperty("2")]
        public short SpeedOverCount { set; get; }

        [JsonProperty("3")]
        public short? StopCount { set; get; }

        [JsonProperty("4")]
        public int? StopTime { set; get; }

        [JsonProperty("5")]
        public float? TotalKm { set; get; }

        [JsonProperty("6")]
        public int PercenInternalBattery { set; get; }

        [JsonProperty("7")]
        public float? Temperature { set; get; }

        [JsonProperty("8")]
        public double Battery { set; get; }

        [JsonProperty("9")]
        public float? Temperature2 { set; get; }

        [JsonProperty("10")]
        public byte MessageId { get; set; }

        [JsonProperty("11")]

        public byte KindID { get; set; } = 1;

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [JsonProperty("12")]
        public DateTime? MCExpried { get; set; }

        /// <summary>
        /// Biển số xe
        /// </summary>
        [JsonProperty("50")]
        public string VehiclePlate { set; get; }

        /// <summary>
        /// Icon của xe
        /// </summary>
        [JsonProperty("51")]
        public IconCode IconVehicle { set; get; }

        /// <summary>
        /// Thời gian hiện tại trên xe
        /// </summary>
        [JsonProperty("52")]
        public DateTime VehicleTime { set; get; }

        /// <summary>
        /// Trạng thái bật tắt của máy
        /// </summary>
        [JsonProperty("53")]
        public int? StatusEngineer { set; get; }

        /// <summary>
        /// Tổng số knm tích lũy từ lúc lắp GPS
        /// </summary>
        [JsonProperty("54")]
        public double? KilometAccumulated { set; get; }

        /// <summary>
        /// Thời gian dừng đỗ mà máy vẫn bật
        /// </summary>
        [JsonProperty("55")]
        public int? ParkingTurnOnVehecle { set; get; }

        /// <summary>
        /// Trạng thái điều hòa
        /// </summary>
        [JsonProperty("56")]
        public bool? AirCondition { set; get; }

        /// <summary>
        /// Trạng thái của cẩu
        /// </summary>
        [JsonProperty("57")]
        public bool? Crane { set; get; }

        /// <summary>
        /// Trạng thái của ben
        /// </summary>
        [JsonProperty("58")]
        public bool? Ben { set; get; }

        /// <summary>
        /// Trạng thái của bồn bê tông
        /// </summary>
        [JsonProperty("59")]
        public bool? Concrete { set; get; }

        /// <summary>
        /// Thông tin địa chỉ
        /// </summary>
        [JsonProperty("60")]
        public string Address { set; get; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [JsonProperty("61")]
        public DateTime ExpriedDate { set; get; }

        /// <summary>
        /// Đã hết hạn hay chưa
        /// </summary>
        [JsonProperty("62")]
        public bool IsExpried { set; get; }

        /// <summary>
        /// Tốc độ GPS
        /// </summary>
        [JsonProperty("63")]
        public int VelocityGPS { set; get; }

        /// <summary>
        /// Thời gian theo GPS
        /// </summary>
        [JsonProperty("64")]
        public DateTime GPSTime { set; get; }

        /// <summary>
        /// Sở quản lý
        /// </summary>
        [JsonProperty("65")]
        public string DepartmentManager { set; get; }

        /// <summary>
        /// Trạng thái của thẻ nhớ
        /// </summary>
        [JsonProperty("66")]
        public MemoryStatus MemoryStatus { set; get; }

        /// <summary>
        /// Danh sách thông tin nhiên liệu
        /// </summary>
        [JsonProperty("67")]
        public double[] NumberLitsOfPerBottle { set; get; }

        /// <summary>
        /// Loại hình vận tải - loại xe
        /// </summary>
        [JsonProperty("68")]
        public byte? TransportTypeID { set; get; }

        /// <summary>
        /// Số điện thoại đăng kí của xe
        /// </summary>
        [JsonProperty("69")]
        public string PhoneNumber { set; get; }

        /// <summary>
        /// Trạng thái ACC
        /// </summary>
        [JsonProperty("70")]
        public bool? AccStatus { set; get; }

        [JsonProperty("71")]
        public uint LogfileProcessModules { set; get; }

        [JsonProperty("72")]
        public int CancelErrorOptions { set; get; }

        [JsonProperty("73")]
        public bool? Door { set; get; }

        [JsonProperty("74")]
        public bool? ConcretePump { set; get; }

        /// <summary>
        /// Số IMEI đăng kí của xe
        /// </summary>
        [JsonProperty("75")]
        public string IMEI { set; get; }

        /// <summary>
        /// Ngày đăng ký dịch vụ
        /// </summary>
        [JsonProperty("76")]
        public DateTime JoinSystemDate { set; get; }

        /// <summary>
        /// Số sim điện thoại đăng ký
        /// </summary>
        [JsonProperty("77")]
        public string SIMPhoneNumber { set; get; }
    }

    public class VehicleHtn
    {
        [JsonProperty("0")]
        public int DoorOpenCount { set; get; }

        [JsonProperty("1")]
        public string DriverLicense { set; get; }

        [JsonProperty("2")]
        public string DriverName { set; get; }

        [JsonProperty("3")]
        public int DrivingViolationCount { set; get; }

        [JsonProperty("4")]
        public string MemoryStatusLabel { set; get; }

        [JsonProperty("5")]
        public int MinutesOfDrivingTimeContinuous { set; get; }

        [JsonProperty("6")]
        public int MinutesOfDrivingTimeInDay { set; get; }

        [JsonProperty("7")]
        public string MobileNumber { set; get; }

        [JsonProperty("8")]
        public string PrinterLineStatus { set; get; }

        [JsonProperty("9")]
        public string VIN { set; get; }
    }

    public class VehicleNl
    {
        [JsonProperty("0")]
        public float Capacity { set; get; }

        [JsonProperty("1")]
        public bool IsUseFuel { set; get; }

        [JsonProperty("2")]
        public float NumberOfLiters { set; get; }
    }
}