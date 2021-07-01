// Original code from https://github.com/javiholcman/Wapps.Forms.Map/
// Cacheing implemented by Gadzair

using Android.Graphics;
using Android.Views;

using Plugin.CurrentActivity;
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

        public static Task<Android.Views.View> ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
        {
            return Task.Run(() =>
            {
                var vRenderer = Platform.CreateRendererWithContext(view, CrossCurrentActivity.Current.Activity);
                var viewGroup = vRenderer.View;
                vRenderer.Tracker.UpdateLayout();
                var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);
                viewGroup.LayoutParameters = layoutParams;
                view.Layout(size);
                viewGroup.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);

                return viewGroup;
            });
        }

        public static Bitmap ConvertViewToBitmap(Android.Views.View v)
        {
            try
            {
                v.Measure(Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                v.Layout(0, 0, v.MeasuredWidth, v.MeasuredHeight);
                Bitmap bitmap = Bitmap.CreateBitmap(v.MeasuredWidth, v.MeasuredHeight, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitmap);
                v.Draw(canvas);
                return bitmap;
            }
            catch (System.Exception)
            {
                return Bitmap.CreateBitmap(v.MeasuredWidth, v.MeasuredHeight, Bitmap.Config.Argb8888);
            }
        }

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