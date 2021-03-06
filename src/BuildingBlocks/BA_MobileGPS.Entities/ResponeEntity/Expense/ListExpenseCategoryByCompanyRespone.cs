using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Entities
{
   public class ListExpenseCategoryByCompanyRespone : BaseModel
    {
        public Guid ID { get; set; }
        public int FK_CompanyID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int OrderNo { get; set; }
        public bool HasLandmark { get; set; }
        //public bool HasPhoto { get; set; }
        private bool hasPhoto =true;
        public bool HasPhoto { get => hasPhoto; set => SetProperty(ref hasPhoto, value); }
    }
}
