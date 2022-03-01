using System;

namespace BA_MobileGPS.Entities
{
    public class FuelsSummariesTotalResponse : ReportBaseResponse
    {
        // STT
        public int RowNumber { get; set; }

        //Tổng Số lần đổ
        public int SumPourCount { get; set; }

        //Tổng Số lần hút
        public int SumSuckCount { get; set; }

        //Tổng Số lít đầu ngày
        public double SumFirstLits { get; set; }

        //Tổng Số lít đổ
        public double SumPourTotal { get; set; }

        //Tổng Số lít hút
        public double SumSuckTotal { get; set; }

        //Tổng Số lít tồn
        public double SumLastLits { get; set; }

        //Tổng Thời gian nổ máy (phút)
        public int SumMinuteOfMachineOn { get; set; }

        //Tổng Thời gian dừng đỗ nổ máy (phút)
        public int SumMinuteOfMachineOnStop { get; set; }

        //Tổng Km GPS
        public double SumTotalKmGps { get; set; }

        //Tổng Định mức quy định
        public double SumNormsOfGasonline { get; set; }

        //Tổng Định mức thực tế
        public double SumRealNormsOfGasonline { get; set; }

        //Tổng Số lít tiêu hao
        public double SumLiterConsumable { get; set; }
    }
}