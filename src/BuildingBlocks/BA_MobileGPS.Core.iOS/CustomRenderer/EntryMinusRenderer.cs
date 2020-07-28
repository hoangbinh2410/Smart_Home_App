using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryMinus), typeof(EntryMinusRenderer))]

namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class EntryMinusRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null) return;
            if (e.NewElement.Keyboard == Keyboard.Numeric)
            {
                Control.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
            }
        }
    }
}