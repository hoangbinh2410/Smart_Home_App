using BA_MobileGPS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness
{
    /// <summary>
    /// Báo cáo chuyến kinh doanh
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// ducpv  19/11/2021   created
    /// </Modified>
    [Serializable]
    public class TransportBusinessResponse : ReportStation
    {
        public int OrderNumber { get; set; }
        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }

        public string DateDisplay => Date == DateTime.MinValue ? string.Empty : DateTimeHelper.FormatDate(Date);

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartTime { get; set; }

        public string StartTimeDisplay => StartTime == DateTime.MinValue ? string.Empty : DateTimeHelper.FormatDateTime(StartTime);

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime EndTime { get; set; }

        public string EndTimeDisplay => EndTime == DateTime.MinValue ? string.Empty : DateTimeHelper.FormatDateTime(EndTime);

        /// <summary>
        /// Tổng thời gian hoạt động
        /// </summary>
        public int TotalTime { get; set; }

        /// <summary>
        /// Địa chỉ bắt đầu
        /// </summary>
        public string StartAddress { get; set; }

        /// <summary>
        /// Địa chỉ kết thúc
        /// </summary>
        public string EndAddress { get; set; }

        ///// <summary>
        ///// Tổng số KmGPS xe đi được trong ngày
        ///// </summary>
        //public double TotalKmGps { get; set; }

        ///// <summary>
        ///// Xung trong ngày
        ///// </summary>
        //public double KmOfPulseMechanical { get; set; }

        /// <summary>
        /// Định mức tiêu thụ nhiên liệu/1km
        /// </summary>
        public double ConstantNorms { set; get; }

        /// <summary>
        /// Nhiên liệu tiêu thụ định mức
        /// </summary>
        public double Norms { set; get; }

        /// <summary>
        /// Số chuyến
        /// </summary>
        public int TripNo { get; set; }

        /// <summary>
        /// Tổng thời gian hoạt động tính theo giây
        /// </summary>
        public int TotalSecond { get; set; }

        public double UseFuel { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte ChangeType { get; set; }
        public int ReaderID { get; set; }

        public string CreatedDateStr => CreatedDate != DateTime.MinValue ? CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;

        public string VehicleTypeName { get; set; }

        public bool OnBen { get; set; }

        public int CurrentBenVelocity { get; set; }

        public int StartAddressId { get; set; }
        public int EndAddressId { get; set; }
    }

    /// <summary>
    /// Báo cáo ra vào trạm
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// ducpv  19/11/2021   created
    /// </Modified>
    [Serializable]
    public class ReportStation : ReportBase
    {
        /// <summary>
        /// ID tram
        /// </summary>
        public int LandmarkID { get; set; }

        /// <summary>
        /// Ten tram
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Thoi gian vao tram
        /// </summary>
        public DateTime TimeInStation { get; set; }

        public string TimeInStationDisplay => TimeInStation == DateTime.MinValue ? string.Empty : DateTimeHelper.FormatDateTime(TimeInStation);

        /// <summary>
        /// Thoi gian ra tram
        /// </summary>
        public DateTime TimeOutStation { get; set; }

        public string TimeOutStationDisplay => TimeOutStation == DateTime.MinValue ? string.Empty : DateTimeHelper.FormatDateTime(TimeOutStation);

        public string Description { get; set; }
        public bool IsOnline { get; set; }

        /// <summary>
        /// Km GPS ra trạm
        /// </summary>
        public double KmOutsideStation { get; set; }

        /// <summary>
        /// Xung Km ra trạm
        /// </summary>
        public double PulseOutSideStation { get; set; }

        public int PeriodInOutStation { get; set; }

        /// <summary>
        /// Tổng số KmGPS xe đi được trong ngày
        /// </summary>
        public double TotalKmGps { get; set; }

        /// <summary>
        /// Xung trong ngày
        /// </summary>
        public double KmOfPulseMechanical { get; set; }

        /// <summary>
        /// Diem roi khoi tram
        /// </summary>
        public string PointOutStation { get; set; }

        /// <summary>
        /// Diem trong tram
        /// </summary>
        public string PointInStation { get; set; }

        /// <summary>
        /// ID loại điểm trạm
        /// </summary>
        public int LandmarkCatalogueID { get; set; }

        public string TimeInStationString { get; set; }
        public string TimeOutStationString { get; set; }
        public int Direction { get; set; }
        public string DateCheck { get; set; }

        /// <summary>
        /// Gets or set TypeVehicle
        /// </summary>
        public string NameType { get; set; }

        /// <summary>
        /// Gets or set MinutesOfAirConditionerOn
        /// </summary>
        public int MinutesOfAirConditionerOn { get; set; }

        /// <summary>
        /// Gets or set MinutesOfAirConditionerOn
        /// </summary>
        public DateTime FromTimeOfAirConditionerOn { get; set; }

        public string FromTimeOfAirConditionerOnStr { get => FromTimeOfAirConditionerOn == new DateTime() ? "" : FromTimeOfAirConditionerOn.ToString("HH:mm"); }
        public string FromTimeOfAirConditionerOnAll { get => FromTimeOfAirConditionerOn == new DateTime() ? "" : FromTimeOfAirConditionerOn.ToString("HH:mm") + " (" + MinutesOfAirConditionerOn.ToString() + ")"; }

        /// <summary>
        /// Gets or set MinutesOfAirConditionerOn
        /// </summary>
        public DateTime ToTimeOfAirConditionerOn { get; set; }

        /// <summary>
        /// Tổng số KmGPS xe đi được trong ngày 3 điểm
        /// </summary>
        public double TotalKmGps3Points { get; set; }

        /// <summary>
        /// Nhiên liệu khi vào trạm
        /// </summary>
        public double FuelInStation { get; set; }

        /// <summary>
        /// Nhiên liệu khi ra trạm
        /// </summary>
        public double FuelOutStation { get; set; }

        /// <summary>
        /// Số lít tiêu thụ
        /// </summary>
        public double TotalLitersOutsideStation { get; set; }

        /// <summary>
        /// Số lít tiêu thụ 3 điểm
        /// </summary>
        public double TotalLitersOutsideStation3Points { get; set; }

        /// <summary>
        /// Định mức tiêu thụ 3 điểm
        /// </summary>
        public double Norms3Points { get; set; }

        public string CompanyName { get; set; }
        public string GroupName { get; set; }
        public int MinutesLimited { get; set; }

        public double StopSeconds { get; set; }

        public double StopMinutes => Math.Round(StopSeconds / 60, MidpointRounding.AwayFromZero);

        public bool UseTimeOutStation { get; set; }

        public bool GoInBackOut { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime FK_Date { get; set; }
    }
    /// <summary>
    /// Cac truong co ban cua bao cao
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// ducpv  19/11/2021   created
    /// </Modified>
    [Serializable]
    public class ReportBase
    {
        /// <summary>
        /// Gets or Sets VehicleID
        /// </summary>
        [JsonProperty("v_id")]
        public long VehicleID { get; set; }

        /// <summary>
        /// Gets or Sets VehiclePlate
        /// </summary>
        public string VehiclePlate { get; set; }

        public int STT { get; set; }

        /// <summary>
        /// Gets or Sets PrivateCode
        /// </summary>
        [JsonProperty("p_code")]
        public string PrivateCode { get; set; }

        /// <summary>
        /// Gets or Sets CompanyID
        /// </summary>
        public int CompanyID { get; set; }

        /// <summary>
        /// Get or Set DriverLicense
        /// </summary>
        public string DriverLicense { get; set; }

        /// <summary>
        /// Get or Set DisplayName
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Get or Set DriverName
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// Gets or Set EmployeeID
        /// </summary>
        public int EmployeeID { get; set; }

        public decimal ConstantMechanicalKm { get; set; }

        public string ResourceCode { get; set; }
        public double? Temperature { get; set; }

        public string TemperatureStr { get; set; }
    }
}
