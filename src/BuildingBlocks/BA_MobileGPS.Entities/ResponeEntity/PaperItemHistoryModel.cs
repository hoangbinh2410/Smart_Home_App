using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    /// <summary>
    /// Thong tin item trong list lịch sử giấy tờ
    /// Các mục comment là dữ liệu không dùng ở UI
    /// </summary>
    public class PaperItemHistoryModel
    {
        public Guid Id { get; set; }
        //public int FKCompanyID { get; set; }
        public int FK_VehicleID { get; set; }
        public Guid FK_PaperCategoryID { get; set; }
        public string PaperCategoryName { get; set; }
        //public string PaperNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ExpireDate { get; set; }
        //public int DayOfAlertBefore { get; set; }
        //public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        //public Guid CreatedByUser { get; set; }
        public string UserCreated { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //public Guid UpdatedByUser { get; set; }
        public string UserUpdated { get; set; }
    }

    public class PaperHistoriesResponse : ResponseBaseV2<DataResponseBase<PaperItemHistoryModel>>
    {

    }
}
