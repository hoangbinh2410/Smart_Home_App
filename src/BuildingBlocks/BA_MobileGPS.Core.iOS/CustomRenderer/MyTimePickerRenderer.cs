using BA_MobileGPS.Core.iOS.CustomRenderer;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TimePicker), typeof(MyTimePickerRenderer))]

namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class MyTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && this.Control != null)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 2))
                {
                    UIDatePicker picker = (UIDatePicker)Control.InputView;
                    picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
                }
            }
        }
    }
}