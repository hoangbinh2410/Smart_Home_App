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
        public long VehicleId { get; set; }
        [JsonProperty("1")]
        public VehicleHtn VehicleHtn { set; get; }

        [JsonProperty("2")]
        public VehicleNl VehicleNl { set; get; }
        
        [JsonProperty("3")]
        public int? StopCount { set; get; }

        [JsonProperty("4")]
        public int? StopTime { set; get; }

        [JsonProperty("5")]
        public float? TotalKm { set; get; }

        [JsonProperty("7")]
        public float? Temperature { set; get; }

        [JsonProperty("9")]
        public float? Temperature2 { set; get; }

        [JsonProperty("10")]
        public byte MessageId { get; set; }

        [JsonProperty("11")]
        public DateTime? MCExpried { get; set; }

        [JsonProperty("12")]
        public string VehiclePlate { set; get; }

        [JsonProperty("13")]
        public IconCode IconVehicle { set; get; }  // xem lai

        /// <summary>
        /// Thời gian hiện tại trên xe
        /// </summary>
        [JsonProperty("14")]
        public DateTime VehicleTime { set; get; }

        /// <summary>
        /// Trạng thái bật tắt của máy
        /// </summary>
        [JsonProperty("15")]
        public int? State { set; get; }

        /// <summary>
        /// Tổng số knm tích lũy từ lúc lắp GPS
        /// </summary>
        [JsonProperty("16")]
        public double? KilometAccumulated { set; get; }

        /// <summary>
        /// Thời gian dừng đỗ mà máy vẫn bật
        /// </summary>
        [JsonProperty("17")]
        public int? ParkingTurnOnVehecle { set; get; }

        /// <summary>
        /// Trạng thái điều hòa
        /// </summary>
        [JsonProperty("18")]
        public bool? AirCondition { set; get; }  // k co o API cu

        /// <summary>
        /// Trạng thái của cẩu
        /// </summary>
        [JsonProperty("19")]
        public bool? Crane { set; get; }

        /// <summary>
        /// Trạng thái của ben
        /// </summary>
        [JsonProperty("20")]
        public bool? Ben { set; get; }

        /// <summary>
        /// Trạng thái của bồn bê tông
        /// </summary>
        [JsonProperty("21")]
        public bool? Concrete { set; get; }

        /// <summary>
        /// Thông tin địa chỉ
        /// </summary>
        [JsonProperty("22")]
        public string Address { set; get; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [JsonProperty("23")]
        public DateTime ExpriedDate { set; get; }

        /// <summary>
        /// Đã hết hạn hay chưa
        /// </summary>
        [JsonProperty("24")]
        public bool IsExpried { set; get; }

        /// <summary>
        /// Tốc độ GPS
        /// </summary>
        [JsonProperty("25")]
        public int VelocityGPS { set; get; }

        /// <summary>
        /// Thời gian theo GPS
        /// </summary>
        [JsonProperty("26")]
        public DateTime GPSTime { set; get; }

        /// <summary>
        /// Sở quản lý
        /// </summary>
        [JsonProperty("27")]
        public string DepartmentManager { set; get; }

        /// <summary>
        /// Trạng thái ACC
        /// </summary>
        [JsonProperty("28")]
        public bool? IsEnableAcc { set; get; }

        [JsonProperty("29")]
        public bool? Door { set; get; }

        [JsonProperty("30")]
        public bool? ConcretePump { set; get; }

        /// <summary>
        /// Số IMEI đăng kí của xe
        /// </summary>
        [JsonProperty("31")]
        public string IMEI { set; get; }

        /// <summary>
        /// Ngày đăng ký dịch vụ
        /// </summary>
        [JsonProperty("32")]
        public DateTime JoinSystemDate { set; get; }

        /// <summary>
        /// Số sim điện thoại đăng kí
        /// </summary>
        [JsonProperty("33")]
        public string SIMPhoneNumber { set; get; }

        /// <summary>
        /// Ngày đăng kiểm
        /// </summary>
        [JsonProperty("34")]
        public DateTime? DateOfRegistration { set; get; }

        /// <summary>
        /// TỔNG KM XE CHẠY TRONG THÁNG
        /// </summary>
        [JsonProperty("35")]
        public float KmInMonth { set; get; }

        ////////////////////////////////////////
        /// <summary>
        /// ẩn hiện với đang đỗ
        /// </summary>
        public bool IsShowParkingVehicleNow
        {
            get
            {
                if (VelocityGPS > 3)
                {
                    return false;
                }
                else
                {
                    return StopTime != null ? true : false;
                }
            }
        }

        ////////////////////////////////////////
        /// <summary>
        /// ẩn hiện với dừng đỗ nổ máy
        /// </summary>
        public bool IsShowParkingTurnOnVehicle
        {
            get
            {
                if (VelocityGPS > 3)
                {
                    return false;
                }
                else
                {
                    return StopTime != null ? true : false;
                }
            }
        }
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
        public string VIN { set; get; }
        [JsonProperty("9")]
        public short SpeedOverCount { set; get; }
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