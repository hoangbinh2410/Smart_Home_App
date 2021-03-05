using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BA_MobileGPS.Entities.ResponeEntity
{
    public class PaperCategoriesResponse : ResponseBaseV2<List<PaperCategory>>
    {

    }


    public class PaperCategory
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Loại giấy tờ
        ///1: đăng kiểm
        ///2: bảo hiểm
        ///3: phù hiệu
        /// </summary>
        public int PaperCategoryType { get; set; }
        public string PaperName { get; set; }
        public string PaperCode { get; set; }
        public string Description { get; set; }
    }

    public class InsuranceCategoriesResponse : ResponseBaseV2<List<InsuranceCategory>>
    {

    }

    public class InsuranceCategory
    {
        public int Id { get; set; }
        public int FK_CompanyID { get; set; }
        public string CategoryName { get; set; }
        public decimal? InsuranceCost { get; set; }
        public decimal? CashIndemnityMax { get; set; }
        public string Description { get; set; }
    }
    public enum PaperCategoryTypeEnum
    {
        [Description("Tất cả loại giấy tờ")]
        None = 0,
        [Description("Đăng kiểm")]
        Registry = 1, 
        [Description("Bảo hiểm")]
        Insurrance = 2,
        [Description("Phù hiệu")]
        Sign = 3
    }

}
