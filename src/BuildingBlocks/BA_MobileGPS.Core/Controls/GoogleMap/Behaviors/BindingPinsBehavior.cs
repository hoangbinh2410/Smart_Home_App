using Prism.Behaviors;

using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    [Preserve(AllMembers = true)]
    public sealed class BindingPinsBehavior : BehaviorBase<Map>
    {
        private static readonly BindablePropertyKey ValuePropertyKey =
            BindableProperty.CreateReadOnly("Value", typeof(ObservableCollection<Pin>), typeof(BindingPinsBehavior), default(ObservableCollection<Pin>));

        public static readonly BindableProperty ValueProperty = ValuePropertyKey.BindableProperty;

        public ObservableCollection<Pin> Value
        {
            get => (ObservableCollection<Pin>)GetValue(ValueProperty);
            private set => SetValue(ValuePropertyKey, value);
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            Value = bindable.Pins as ObservableCollection<Pin>;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            Value = null;
        }
    }
}