using System;

namespace BA_MobileGPS.Entities
{
    public class StatusAlertRequestModel
    {
        /// <summary>
        /// id cảnh báo
        /// </summary>
        /// <value>
        /// The pk alert detail identifier.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  1/4/2019   created
        /// </Modified>
        public long Id { set; get; }

        /// <summary>
        /// giá trị flag trạng thái cảnh báo
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  1/4/2019   created
        /// </Modified>
        public StatusAlert Status { set; get; }

        /// <summary>
        /// nội dung xử lý
        /// </summary>
        /// <value>
        /// The content of the proccess.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  1/4/2019   created
        /// </Modified>
        public string ProccessContent { set; get; }

        /// <summary>
        /// userid cập nhật
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  1/4/2019   created
        /// </Modified>
        public Guid UserID { set; get; }

        public int CompanyID { get; set; }
        #region Thông tin ID cuốc cảnh báo

        public long FK_VehicleID { set; get; }
        public DateTime StartTime { set; get; }
        public int FK_AlertTypeID { set; get; }

        #endregion Thông tin ID cuốc cảnh báo
    }

    public enum StatusAlert : int
    {
        Null = 0,
        Readed = 1, // đánh dấu đã đọc
        Process = 3 // đánh dấu đã xử lý
    }
}