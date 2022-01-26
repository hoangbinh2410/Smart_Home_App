using System;

namespace BA_MobileGPS.Entities
{
    public class StopParkingVehicleModel : ReportBaseResponse
    {
        public string PrivateCode { get; set; } // Biển số xe

        public string DriverName { get; set; } //Tên lái xe

        public string EmployeeCode { get; set; } //Mã nhân viên

        public string Phone { get; set; } //Số điện thoại

        public int TotalTimeStop { get; set; } //Thời gian (phút)

        public string StopParkingTime { get; set; } //Thời gian dừng đỗ

        public int MinutesOfManchineOn { get; set; } //Nổ máy khi dừng

        public int MinutesOfAirConditioningOn { get; set; }//Bật điều hoà khi dùng

        public string Address { get; set; } //Địa điểm dừng

        public string Temperature { get; set; } //Hiện trạng nhiệt độ
    }
}