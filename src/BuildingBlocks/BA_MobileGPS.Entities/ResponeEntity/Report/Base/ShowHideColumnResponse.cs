namespace BA_MobileGPS.Entities
{
    public class ShowHideColumnResponse : CommonResponseRealBase
    {
        public int IDTable { get; set; }

        public int IDColumn { get; set; }

        public bool Value { get; set; }  // true: show || false : Hide
    }
}