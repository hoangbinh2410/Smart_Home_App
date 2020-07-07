using System;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls.TourGuide
{
    public interface IOverlayService
    {
        void AddOverlay(View onView, ShowCaseConfig config, Action action);

        void HideOverlay();
    }
}