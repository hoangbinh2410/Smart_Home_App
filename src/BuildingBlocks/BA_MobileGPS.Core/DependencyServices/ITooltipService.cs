using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public interface ITooltipService
    {
        void ShowTooltip(View onView, TooltipConfig config);

        void HideTooltip(View onView);
    }

    public class TooltipConfig
    {
        public string Text { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public TooltipPosition Position { get; set; }
    }

    public enum TooltipPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }
}