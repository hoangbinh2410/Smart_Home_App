using Prism.Behaviors;

using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class BindingMapRegionBehavior : BehaviorBase<Map>
    {
        private static readonly BindablePropertyKey ValuePropertyKey = BindableProperty.CreateReadOnly("Value", typeof(MapRegion), typeof(BindingMapRegionBehavior), default(MapRegion));

        public static readonly BindableProperty ValueProperty = ValuePropertyKey.BindableProperty;

        public MapRegion Value
        {
            get => (MapRegion)GetValue(ValueProperty);
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
            if (args.PropertyName == "Region")
            {
                Value = AssociatedObject.Region;
            }
        }
    }
}