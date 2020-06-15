using System;

namespace BA_MobileGPS.Entities
{
    [Serializable]
    public class ActivityDetailsModel : ReportBaseResponse
    {
        //Loại xe
        public string NameType { get; set; }

        //Biển số xe
        public string PrivateCode { get; set; }

        //Ngày tháng
        public DateTime Date { get; set; }

        //Thời gian bắt đầu
        public TimeSpan StartTimes { get; set; }

        //Thời gian kết thúc
        public TimeSpan EndTimes { get; set; }

        //Thời gian hoạt động
        public TimeSpan TotalTimes { get; set; } 
        
        //Km (GPS)
        public float TotalKm { get; set; } 
        
        //Km cơ
        public double KmOfMechanical { get; set; } 
        
        //Định mức nhiên liệu
        public double ConstantNorms { get; set; } 
        
        //Nhiên liệu tiêu thụ định mức
        public double Norms { get; set; } 

        //Cuốc bù
        public short DataType { get; set; }

        public string DataTypeStr => DataType == 0 ? "X" : string.Empty;
    }
}