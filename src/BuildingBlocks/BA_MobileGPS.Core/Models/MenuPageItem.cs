using Xamarin.Forms;

namespace BA_MobileGPS.Core.Models
{
    public class MenuPageItem
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public bool UseModalNavigation { get; set; }

        public MenuKeyType MenuType { get; set; }

        public Color IconColor { get; set; }

        public bool IsEnable { get; set; }
    }

    public enum MenuKeyType
    {
        Route,
        VehicleDetail,
        Images,
        Video,
        VideoPlayback,
        Online,
        HelpCustomer,
        ExportVideo,
    }
}