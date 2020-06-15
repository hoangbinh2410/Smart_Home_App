// Original code from https://github.com/javiholcman/Wapps.Forms.Map/
// Cacheing implemented by Gadzair

using Android.Graphics;
using Android.Views;

using Plugin.CurrentActivity;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace BA_MobileGPS.Core.Droid
{
    public static class Utils
    {
        /// <summary>
        /// convert from dp to pixels
        /// </summary>
        /// <param name="dp">Dp.</param>
        public static int DpToPx(float dp)
        {
            var metrics = CrossCurrentActivity.Current.AppContext.Resources.DisplayMetrics;
            return (int)(dp * metrics.Density);
        }

        public static Task<Android.Views.View> ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size, IVisualElementRenderer vRenderer)
        {
            return Task.Run(() =>
            {
                var renderer = Platform.CreateRendererWithContext(view, CrossCurrentActivity.Current.Activity);
                var nativeView = renderer.View;
                renderer.Tracker.UpdateLayout();
                var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);
                nativeView.LayoutParameters = layoutParams;
                view.Layout(size);
                nativeView.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
                //await FixImageSourceOfImageViews(viewGroup as ViewGroup); // Not sure why this was being done in original
                return nativeView;
            });
        }

        public static Bitmap ConvertViewToBitmap(Android.Views.View v)
        {
            v.SetLayerType(LayerType.Hardware, null);
            v.DrawingCacheEnabled = true;

            v.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
            v.Layout(0, 0, v.MeasuredWidth, v.MeasuredHeight);

            v.BuildDrawingCache(true);
            Bitmap b = Bitmap.CreateBitmap(v.GetDrawingCache(true));
            v.DrawingCacheEnabled = false; // clear drawing cache
            return b;
        }

        private static LinkedList<string> lruTracker = new LinkedList<string>();
        private static ConcurrentDictionary<string, Android.Gms.Maps.Model.BitmapDescriptor> cache = new ConcurrentDictionary<string, Android.Gms.Maps.Model.BitmapDescriptor>();

        public static Task<Android.Gms.Maps.Model.BitmapDescriptor> ConvertViewToBitmapDescriptor(Android.Views.View v)
        {
            return Task.Run(() =>
            {
                var bmp = ConvertViewToBitmap(v);
                var img = Android.Gms.Maps.Model.BitmapDescriptorFactory.FromBitmap(bmp);
                return img;
            });
        }
    }
}