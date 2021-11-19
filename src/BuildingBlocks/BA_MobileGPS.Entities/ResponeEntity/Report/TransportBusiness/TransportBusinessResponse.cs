using BA_MobileGPS.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness
{

    [Serializable]
    public class TransportBusinessResponse : ReportBaseResponse
    {
        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }

        public string DateDisplay => Date == DateTime.MinValue ? string.Empty : DateTimeHelper.FormatDate(Date);

        /// <summary>
        /// Địa chỉ bắt đầu
        /// </summary>
        public string StartAddress { get; set; }

        /// <summary>
        /// Địa chỉ kết thúc
        /// </summary>
        public string EndAddress { get; set; }

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

        ///// <summary>
        ///// Tổng số KmGPS xe đi trong chuyến
        ///// </summary>
        //public double TotalKmGps { get; set; }
        public double TotalKmGps { set; get; }

        ///// <summary>
        ///// Tổng số KmGPS xe đi trong chuyến
        ///// </summary>
        //public double TotalKmGps { get; set; }
        public double KmOfPulseMechanical { set; get; }

        /// <summary>
        /// Nhiên liệu tiêu thụ 
        /// </summary>
        public double FuelOutStation { set; get; }

        /// <summary>
        /// Định mức tiêu thụ nhiên liệu/1km
        /// </summary>
        public double ConstantNorms { set; get; }

        /// <summary>
        /// Nhiên liệu tiêu thụ định mức
        /// </summary>
        public double Norms { set; get; }
    }
}
