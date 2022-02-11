using System;

namespace BA_MobileGPS.Entities
{
    public class FuelsSummariesModel : ReportBaseResponse
    {
        // STT
        public int RowNumber { get; set; }

        //Biển số xe
        public string PrivateCode { get; set; }

        //Ngày tháng
        public DateTime Date { get; set; }

        //Số lần đổ
        public int PourCount { get; set; }

        //Số lần hút
        public int SuckCount { get; set; }

        //Số lít đầu ngày
        public double FirstLits { get; set; }

        //Số lít đổ
        public double PourTotal { get; set; }

        //Số lít hút
        public double SuckTotal { get; set; }

        //Số lít tồn
        public double LastLits { get; set; }

        //Thời gian nổ máy (phút)
        public int MinuteOfMachineOn { get; set; }

        // Thời gian dừng đỗ nổ máy (phút)
        public int MinuteOfMachineOnStop { get; set; }

        //Tổng Km GPS
        public double TotalKmGps { get; set; }

        //Định mức quy định
        public double NormsOfGasonline { get; set; }

        //Định mức thực tế
        public double RealNormsOfGasonline { get; set; }

        //Số lít tiêu hao
        public double LiterConsumable { get; set; }
    }
}