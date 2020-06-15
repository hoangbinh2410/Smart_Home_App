namespace BA_MobileGPS.Entities
{
    public class MobileResourceRespone : CommonResponseRealBase
    {
        public string CodeName { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Giá trị
        /// </summary>
        public string Value { get; set; }

        public int FK_LanguageID { get; set; }
    }
}