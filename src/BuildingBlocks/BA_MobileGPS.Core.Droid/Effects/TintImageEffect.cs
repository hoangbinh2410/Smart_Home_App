using System.Linq;
using Android.Graphics;
using Android.Widget;
using BA_MobileGPS.Core.Droid.Effects;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsTintImageEffect = BA_MobileGPS.Core.Effects.TintImageEffect;


[assembly: ExportEffect(typeof(TintImageEffect), BA_MobileGPS.Core.Effects.TintImageEffect.Name)]
namespace BA_MobileGPS.Core.Droid.Effects
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (FormsTintImageEffect)Element.Effects.FirstOrDefault(e => e is FormsTintImageEffect);

                if (effect == null || !(Control is ImageView image))
                    return;

                var filter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                image.SetColorFilter(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
}