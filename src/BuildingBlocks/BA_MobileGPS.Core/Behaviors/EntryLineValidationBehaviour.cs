using Prism.Behaviors;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Behaviors
{
    public class EntryLineValidationBehaviour : BehaviorBase<Entry>
    {
        #region StaticFields

        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(EntryLineValidationBehaviour), true, BindingMode.Default, null, (bindable, oldValue, newValue) => OnIsValidChanged(bindable, newValue));

        #endregion StaticFields

        #region Properties

        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }

        #endregion Properties

        #region StaticMethods

        private static void OnIsValidChanged(BindableObject bindable, object newValue)
        {
            if (bindable is EntryLineValidationBehaviour IsValidBehavior &&
                 newValue is bool IsValid)
            {
                IsValidBehavior.AssociatedObject.PlaceholderColor = IsValid ? Color.Default : Color.Red;
            }
        }

        #endregion StaticMethods
    }
}