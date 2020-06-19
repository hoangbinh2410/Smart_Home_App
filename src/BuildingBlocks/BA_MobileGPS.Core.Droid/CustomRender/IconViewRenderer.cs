using Android.Content;
using Android.Graphics;
using Android.Widget;

using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.CustomRenderer;
using BA_MobileGPS.Utilities;

using System.ComponentModel;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRendererAttribute(typeof(IconView), typeof(IconViewRenderer))]

namespace BA_MobileGPS.Core.CustomRenderer
{
    public class IconViewRenderer : ViewRenderer<IconView, ImageView>
    {
        private bool _isDisposed;

        public IconViewRenderer(Context context) : base(context)
        {
            base.AutoPackage = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                SetNativeControl(new ImageView(Context));
            }
            UpdateBitmap(e.OldElement);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
            else if (e.PropertyName == IconView.ForegroundProperty.PropertyName)
            {
                UpdateBitmap(null);
            }
        }

        private void UpdateBitmap(IconView previous = null)
        {
            if (!_isDisposed)
            {
                try
                {
                    var d = Context.GetDrawable(Element.Source).Mutate();
                    var colorFilter = new PorterDuffColorFilter(Element.Foreground.ToAndroid(), PorterDuff.Mode.SrcIn);
                    d.SetColorFilter(colorFilter);
                    Control.SetImageDrawable(d);
                    ((IVisualElementController)Element).NativeSizeChanged();
                }
                catch (System.Exception ex)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                }
            }
        }
    }
}