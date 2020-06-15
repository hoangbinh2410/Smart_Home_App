using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class InfoWindowLongClickedToCommandBehavior : EventToCommandBehaviorBase
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.InfoWindowLongClicked += OnInfoWindowLongClicked;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.InfoWindowLongClicked -= OnInfoWindowLongClicked;
        }

        private void OnInfoWindowLongClicked(object sender, InfoWindowLongClickedEventArgs infoWindowLongClickedEventArgs)
        {
            Command?.Execute(infoWindowLongClickedEventArgs);
        }
    }
}