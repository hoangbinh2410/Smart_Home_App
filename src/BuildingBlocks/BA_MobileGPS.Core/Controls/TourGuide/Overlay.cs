using System;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls.TourGuide
{
    public class Overlay
    {
        public Overlay()
        {
            OverlayService = DependencyService.Get<IOverlayService>();
        }

        private IOverlayService OverlayService { get; }

        public void Show(View onView, ShowCaseConfig config, Action action)
        {
            OverlayService.AddOverlay(onView, config, action);
        }

        public void Hide()
        {
            OverlayService.HideOverlay();
        }
    }
}