using Android.Content;
using BA_MobileGPS.Core.Droid.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRendererAttribute(typeof(WebView), typeof(TransparentWebViewRenderer))]

namespace BA_MobileGPS.Core.Droid.CustomRender
{
    public class TransparentWebViewRenderer : WebViewRenderer
    {
        public TransparentWebViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            // Setting the background as transparent
            this.Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}