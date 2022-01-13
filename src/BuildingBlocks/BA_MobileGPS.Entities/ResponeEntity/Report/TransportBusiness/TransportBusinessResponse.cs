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
    public class TransportBusinessResponse : ReportBaseResponse
    {
        public int RowNumber { get; set; }       
        public string Name { get; set; }
        public string PrivateCode { get; set; }
        public int TotalTime { get; set; }
        public double TotalKmGps { get; set; }
        ///// <summary>
        ///// KM Cơ
        ///// </summary>
        public double KmOfPulseMechanical { get; set; }

        /// <summary>
        /// NL tiêu thụ
        /// </summary>
        public double UseFuel { get; set; }

        /// <summary>
        /// Định mức tiêu thụ nhiên liệu/1km
        /// </summary>
        public double ConstantNorms { set; get; }

        /// <summary>
        /// Nhiên liệu tiêu thụ định mức
        /// </summary>
        public double Norms { set; get; }


        /// <summary>
        /// Nhiên liệu khi vào trạm
        /// </summary>
        public double FuelInStation { get; set; }

        /// <summary>
        /// Nhiên liệu khi ra trạm
        /// </summary>
        public double FuelOutStation { get; set; }

        /// <summary>
        /// Thoi gian vao tram
        /// </summary>
        public DateTime TimeInStation { get; set; }

        /// <summary>
        /// Thoi gian ra tram
        /// </summary>

        public DateTime TimeOutStation { get; set; }

        /// <summary>
        /// Km GPS ra trạm
        /// </summary>
        public double KmOutsideStation { get; set; }

        /// <summary>
        /// Xung Km ra trạm
        /// </summary>
        public double PulseOutSideStation { get; set; }

        public decimal ConstantMechanicalKm { get; set; }
    }
}
