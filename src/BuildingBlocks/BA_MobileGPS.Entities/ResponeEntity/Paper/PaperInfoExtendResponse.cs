using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class PaperInfoExtendResponse
    {
        public Guid Id { get; set; }

        public string WarrantyCompany { get; set; }

        public decimal? Cost { get; set; }

        public string Contact { get; set; }

        public int? FK_InsuranceCategoryID { get; set; }
    }
}
