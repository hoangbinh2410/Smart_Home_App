using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls.TourGuide
{
    public class TourGuideBuilder
    {
        private readonly TourGuide tourGuide = new TourGuide();

        public TourGuideBuilder()
        {
        }

        public TourGuideBuilder AddStep(View _Widget, ShowCaseConfig _config)
        {
            tourGuide.GuideSets.Add(new GuideSet(_Widget, _config));
            return this;
        }

        public TourGuide Build()
        {
            return tourGuide;
        }
    }
}