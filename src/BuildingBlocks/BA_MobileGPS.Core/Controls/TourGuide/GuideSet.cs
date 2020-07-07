using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls.TourGuide
{
    public class GuideSet
    {
        public View Widget { set; get; }

        public ShowCaseConfig Config { set; get; }

        public GuideSet(View _Widget, ShowCaseConfig _config)
        {
            Widget = _Widget;
            Config = _config;
        }
    }
}