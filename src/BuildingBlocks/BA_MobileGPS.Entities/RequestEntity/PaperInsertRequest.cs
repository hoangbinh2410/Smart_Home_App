using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.RequestEntity
{
    public class PaperRegistrationInsertRequest
    {
        public PaperBasicInfor PaperInfo { get; set; }
        public string WarrantyCompany { get; set; }
        public decimal Cost { get; set; }
    }

    public class PaperInsuranceInsertRequest
    {
        public PaperBasicInfor PaperInfo { get; set; }
        public string WarrantyCompany { get; set; }
        public decimal Cost { get; set; }
        public string Contact { get; set; }
        public int FK_InsuranceCategoryID { get; set; }
    }

    public class PaperCabSignInsertRequest : PaperBasicInfor
    {
    }

    public class PaperBasicInfor
    {
        public Guid Id { get; set; }
        public int FK_CompanyID { get; set; }
        public long FK_VehicleID { get; set; }
        public string PaperNumber { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime ExpireDate { get; set; }
        public int DayOfAlertBefore { get; set; }
        public string Description { get; set; }
        public Guid CreatedByUser { get; set; }
    }


}
