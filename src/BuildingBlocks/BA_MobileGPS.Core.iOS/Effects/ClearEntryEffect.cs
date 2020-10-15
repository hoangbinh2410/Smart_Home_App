using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(BA_MobileGPS.Core.iOS.Effects.ClearEntryEffect), "ClearEntryEffect")]

namespace BA_MobileGPS.Core.iOS.Effects
{
    public class ClearEntryEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            ConfigureControl();
        }

        protected override void OnDetached()
        {
        }

        private void ConfigureControl()
        {
            ((UITextField)Control).ClearButtonMode = UITextFieldViewMode.WhileEditing;
        }
    }
}