using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class PapersInforInsertResponse : ResponseBaseV2<PapersIdResponse>
    {

    }

    public class PapersIdResponse
    {
        public Guid PK_PaperInfoID { get; set; }
    }

    public class BasePaperInforResponse : ResponseBaseV2<BasePaperInfor>
    {

    }

    public class BasePaperInfor
    {
        public string Id { get; set; }
        public int FK_CompanyID { get; set; }
        public int FK_VehicleID { get; set; }
        public string FK_PaperCategoryID { get; set; }
        public string PaperNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ExpireDate { get; set; }
        public int DayOfAlertBefore { get; set; }
        public string Description { get; set; }
        public PaperInfoExtend paperInfoExtend { get; set; }
    }

    public class PaperInfoExtend
    {
        public string Id { get; set; }
        public string WarrantyCompany { get; set; }
        public decimal Cost { get; set; }
        public string Contact { get; set; }
        public int FK_InsuranceCategoryID { get; set; }
    }
}
