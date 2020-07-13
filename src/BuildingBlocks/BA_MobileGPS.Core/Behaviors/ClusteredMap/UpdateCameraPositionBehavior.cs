using System;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Behaviors.ClusteredMap
{
    public class UpdateCameraPositionBehavior : BehaviorBase<Map>
    {
        public static readonly BindableProperty CameraUpdateProperty =
            BindableProperty.Create("CameraUpdate", typeof(CameraUpdate), typeof(UpdateCameraPositionBehavior), default(CameraUpdate), propertyChanged: OnCameraUpdateChanged);

        public static readonly BindableProperty DurationProperty =
            BindableProperty.Create("Duration", typeof(TimeSpan?), typeof(UpdateRegionBehavior), null);

        public CameraUpdate CameraUpdate => (CameraUpdate)GetValue(CameraUpdateProperty);

        public TimeSpan? Duration => (TimeSpan?)GetValue(DurationProperty);

        private static void OnCameraUpdateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null)
                return;

            var behavior = (UpdateCameraPositionBehavior)bindable;

            if (behavior.CameraUpdate == null)
                return;

            if (behavior.Duration == null)
            {
                behavior.AssociatedObject.MoveCamera(behavior.CameraUpdate);
            }
            else
            {
                behavior.AssociatedObject.AnimateCamera(behavior.CameraUpdate, behavior.Duration);
            }
        }
    }
}