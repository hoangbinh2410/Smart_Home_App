﻿using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class CameraIdledToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.CameraIdled += OnCameraIdled;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.CameraIdled -= OnCameraIdled;
        }

        private void OnCameraIdled(object sender, CameraIdledEventArgs cameraIdledEventArgs)
        {
            Command?.Execute(cameraIdledEventArgs);
        }
    }
}