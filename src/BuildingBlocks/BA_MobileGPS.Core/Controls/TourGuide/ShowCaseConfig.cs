using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls.TourGuide
{
    public class ShowCaseConfig
    {
        public string Id { get; set; }

        public string PimaryText { get; set; }

        public string SecondaryText { get; set; }

        public Color BackgroundColor { get; set; }

        public Color TargetHolderColor { get; set; } = Color.White;

        public VerticalPosition TextVerticalPosition { get; set; }

        public HorizontalPosition TextHorizontalPosition { get; set; }

        public FocusShape FocusShape { get; set; }
    }

    public enum FocusShape
    {
        Circle,
        RoundedRectangle
    }

    public enum VerticalPosition
    {
        Top,
        Center,
        Bottom
    }

    public enum HorizontalPosition
    {
        Left,
        Center,
        Right
    }
}