using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class PaperInfoDetailResponse
    {
        public Guid Id { get; set; }

        public int FK_CompanyID { get; set; }

        public long FK_VehicleID { get; set; }

        public Guid FK_PaperCategoryID { get; set; }

        public string PaperNumber { get; set; }

        public DateTime DateOfIssue { get; set; }

        public DateTime ExpireDate { get; set; }

        public byte DayOfAlertBefore { get; set; }

        public string Description { get; set; }

        public PaperInfoExtendResponse PaperInfoExtend { get; set; }
    }
}
