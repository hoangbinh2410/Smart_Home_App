﻿using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class MapLongClickedToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.MapLongClicked += OnMapLongClicked;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.MapLongClicked -= OnMapLongClicked;
        }

        private void OnMapLongClicked(object sender, MapLongClickedEventArgs args)
        {
            Command?.Execute(args);
        }
    }
}