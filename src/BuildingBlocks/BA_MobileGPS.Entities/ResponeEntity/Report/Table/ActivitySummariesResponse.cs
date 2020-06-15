using System;

namespace BA_MobileGPS.Entities
{
    [Serializable]
    public class ActivitySummariesModel : ReportBaseResponse
    {
        //Loại xe
        public string NameType { get; set; }

        //Ngày lấy báo cáo
        public DateTime FK_Date { get; set; }

        public string JoinFK_Date { get; set; }

        //Thời gian bắt đầu
        public TimeSpan StartTimes { get; set; }

        public string JoinStartTimes { get; set; }

        //Thời gian kết thúc
        public TimeSpan EndTimes { get; set; }

        public string JoinEndTimes { get; set; }

        //Thời gian lăn bánh
        public TimeSpan ActivityTimes { get; set; } 

        public string JoinActivityTimes { get; set; }

        //Thời gian dừng đỗ
        public TimeSpan TotalTimeStops { get; set; }

        public string JoinTotalTimeStops { get; set; }
        //Km (GPS)
        public float TotalKmGps { get; set; } 

        //Km cơ
        public double KmOfMechanical { get; set; }

        //Số lần dừng đỗ
        public int StopCount { get; set; } 

        //Bật điều hoà khi dùng
        public int MinutesOfAirConditioningOn { get; set; }

        //Vận tốc cực đại
        public int Vmax { get; set; } 

        //Vận tốc trung bình
        public int Varg { get; set; } 
    }
}