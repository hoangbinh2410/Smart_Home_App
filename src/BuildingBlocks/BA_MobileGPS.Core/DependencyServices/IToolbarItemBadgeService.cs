using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public interface IToolbarItemBadgeService
    {
        void SetBadge(Page page, ToolbarItem item, string value, Color backgroundColor, Color textColor);
    }
}