using System;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls
{
    public class NullableDatePicker : DatePicker
    {
        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(NullableDatePicker), "../../..");

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(nameof(NullableDate), typeof(DateTime?), typeof(NullableDatePicker), null, defaultBindingMode: BindingMode.TwoWay);

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        public string originalFormat = null;

        public NullableDatePicker()
        {
            Format = "d";
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                originalFormat = Format;
                UpdateDate();
            }
        }

        private void UpdateDate()
        {
            if (NullableDate != null)
            {
                if (originalFormat != null)
                {
                    Format = originalFormat;
                }
            }
            else
            {
                Format = PlaceHolder;
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused && (Date.ToString("d") == DateTime.Now.ToString("d"))))
            {
                AssignValue();
            }

            if (propertyName == NullableDateProperty.PropertyName && NullableDate.HasValue)
            {
                Date = NullableDate.Value;
                if (Date.ToString(originalFormat) == DateTime.Now.ToString(originalFormat))
                {
                    //this code was done because when date selected is the actual date the"DateProperty" does not raise
                    UpdateDate();
                }
            }
        }

        public void CleanDate()
        {
            NullableDate = null;
            UpdateDate();
        }

        public void AssignValue()
        {
            NullableDate = Date;
            UpdateDate();
        }
        public static BindableProperty PlaceholderColorProperty =
       BindableProperty.Create(nameof(PlaceholderColor), typeof(string), typeof(NullableDatePicker), "#CCCCCC", BindingMode.TwoWay);
        public string PlaceholderColor
        {
            get { return (string)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }
    }
}