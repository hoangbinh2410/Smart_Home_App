using Android.App;
using Android.Content;
using Android.Widget;

using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.Droid.CustomRenderer;

using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NullableDatePicker), typeof(NullableDatePickerRenderer))]

namespace BA_MobileGPS.Core.Droid.CustomRenderer
{
    public class NullableDatePickerRenderer : ViewRenderer<NullableDatePicker, EditText>
    {
        private DatePickerDialog dialog;

        public NullableDatePickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NullableDatePicker> e)
        {
            base.OnElementChanged(e);

            SetNativeControl(new EditText(Context));

            if (Control == null || e.NewElement == null)
                return;          
            var entry = Element;
            var customPicker = e.NewElement as NullableDatePicker;
            Control.Click += OnPickerClick;
            Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
            Control.KeyListener = null;
            Control.FocusChange += OnPickerFocusChange;
            Control.Enabled = Element.IsEnabled;
           // Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));
            //Control.TextColors
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = Element;

                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }                                         
        private void OnPickerFocusChange(object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Click -= OnPickerClick;
                Control.FocusChange -= OnPickerFocusChange;

                if (dialog != null)
                {
                    dialog.Hide();
                    dialog.Dispose();
                    dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        private void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        private void SetDate(DateTime date)
        {
            Control.Text = date.ToString(Element.Format);
            Element.Date = date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(Element.Date.Year, Element.Date.Month - 1, Element.Date.Day);
            dialog.Show();
        }

        private void CreateDatePickerDialog(int year, int month, int day)
        {
            NullableDatePicker view = Element;
            dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                dialog = null;
            }, year, month, day);

            dialog.SetButton("Done", (sender, e) =>
            {
                Element.Format = Element.originalFormat;
                SetDate(dialog.DatePicker.DateTime);
                Element.AssignValue();
            });
            dialog.SetButton2("Clear", (sender, e) =>
            {
                Element.CleanDate();
                Control.Text = Element.Format;
            });
        }
        private float GetRed(string color)
        {
            Color c = Color.FromHex(color);
            return (float)c.R;
        }

    }
}