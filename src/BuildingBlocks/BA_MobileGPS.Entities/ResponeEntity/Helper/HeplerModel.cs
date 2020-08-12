namespace BA_MobileGPS.Entities
{
    public class HeplerModel
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public GuideType GuideType { get; set; }

        public bool IsShow { get; set; }
    }

    public class HeplerMenuModel
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public bool IsShow { get; set; }
    }

    public enum GuideType
    {
        //hướng dẫn sử dụng giám sát xe
        Online = 0,

        //Hưỡng dẫn Sử dụng lộ trình
        Router = 1,

        //Hưỡng dẫn camera
        Camera = 2,

        //Hưỡng dẫn danh sách xe nợ phí
        ListVihicleDebt = 3,

        //Hưỡng dẫn danh sách xe
        ListVihicle = 4,

        Report = 5,
    }
}