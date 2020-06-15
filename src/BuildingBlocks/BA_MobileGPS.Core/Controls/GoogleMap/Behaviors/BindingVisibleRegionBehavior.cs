using Prism.Behaviors;

using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class BindingVisibleRegionBehavior : BehaviorBase<Map>
    {
        private static readonly BindablePropertyKey ValuePropertyKey = BindableProperty.CreateReadOnly("Value", typeof(MapSpan), typeof(BindingVisibleRegionBehavior), default(MapSpan));

        public static readonly BindableProperty ValueProperty = ValuePropertyKey.BindableProperty;

        public MapSpan Value
        {
            get => (MapSpan)GetValue(ValueProperty);
            private set => SetValue(ValuePropertyKey, value);
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.PropertyChanged += MapOnPropertyChanged;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            bindable.PropertyChanged -= MapOnPropertyChanged;
            base.OnDetachingFrom(bindable);
        }

        private void MapOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "VisibleRegion")
            {
                Value = AssociatedObject.VisibleRegion;
            }
        }
    }
}