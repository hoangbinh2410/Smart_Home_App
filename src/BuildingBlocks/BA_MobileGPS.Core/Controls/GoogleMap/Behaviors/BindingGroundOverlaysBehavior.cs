using Prism.Behaviors;

using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class BindingGroundOverlaysBehavior : BehaviorBase<Map>
    {
        private static readonly BindablePropertyKey ValuePropertyKey =
            BindableProperty.CreateReadOnly("Value", typeof(ObservableCollection<GroundOverlay>), typeof(BindingPinsBehavior), default(ObservableCollection<GroundOverlay>));

        public static readonly BindableProperty ValueProperty = ValuePropertyKey.BindableProperty;

        public ObservableCollection<GroundOverlay> Value
        {
            get => (ObservableCollection<GroundOverlay>)GetValue(ValueProperty);
            private set => SetValue(ValuePropertyKey, value);
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            Value = bindable.GroundOverlays as ObservableCollection<GroundOverlay>;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            Value = null;
        }
    }
}