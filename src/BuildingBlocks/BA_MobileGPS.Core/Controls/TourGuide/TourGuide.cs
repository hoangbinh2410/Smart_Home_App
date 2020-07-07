using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Controls.TourGuide
{
    public class TourGuide
    {
        public List<GuideSet> GuideSets { set; get; }

        public TourGuide()
        {
            GuideSets = new List<GuideSet>();
        }

        private Action InitAction(int indexOfItem)
        {
            if (indexOfItem == GuideSets.Count)
                return null;

            var currentItem = GuideSets[indexOfItem];

            return new Action(async () =>
            {
                await Task.Delay(500);

                var nextAction = InitAction(indexOfItem + 1);
                Overlay overlay = new Overlay();
                overlay.Show(currentItem.Widget, currentItem.Config, nextAction);
            });
        }

        public void StartTour()
        {
            InitAction(0)?.Invoke();
        }
    }
}