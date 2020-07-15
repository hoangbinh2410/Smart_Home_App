using Syncfusion.SfChart.XForms;

namespace BA_MobileGPS.Core.Behaviors.Trackball
{
    public class ChartTrackballBehaviorExt : ChartTrackballBehavior
    {
        protected override void OnTouchDown(float pointX, float pointY)
        {
            if (!HitTest(pointX, pointY))
            {
                Show(pointX, pointY);
            }
        }

        protected override void OnTouchMove(float pointX, float pointY)
        {
            Show(pointX, pointY);
        }

        protected override void OnTouchUp(float pointX, float pointY)
        {
            Show(pointX, pointY);
        }
    }
}