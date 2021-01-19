using FFImageLoading.Forms;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.Controls
{
    public class ZoomImage : CachedImage
    {
        public ZoomImage()
        {
        }

        public static readonly BindableProperty ZoomEnabledProperty =
            BindableProperty.Create(nameof(ZoomEnabled), typeof(bool), typeof(ZoomImage), true);

        public bool ZoomEnabled
        {
            get { return (bool)GetValue(ZoomEnabledProperty); }
            set { SetValue(ZoomEnabledProperty, value); }
        }

        public static readonly BindableProperty ScrollEnabledProperty =
            BindableProperty.Create(nameof(ScrollEnabled), typeof(bool), typeof(ZoomImage), true);

        public bool ScrollEnabled
        {
            get { return (bool)GetValue(ScrollEnabledProperty); }
            set { SetValue(ScrollEnabledProperty, value); }
        }

        public static readonly BindableProperty DoubleTapToZoomEnabledProperty =
            BindableProperty.Create(nameof(DoubleTapToZoomEnabled), typeof(bool), typeof(ZoomImage), true);

        public bool DoubleTapToZoomEnabled
        {
            get { return (bool)GetValue(DoubleTapToZoomEnabledProperty); }
            set { SetValue(DoubleTapToZoomEnabledProperty, value); }
        }

        public static readonly BindableProperty TapZoomScaleProperty =
            BindableProperty.Create(nameof(TapZoomScale), typeof(double), typeof(ZoomImage), 2.0);

        public double TapZoomScale
        {
            get { return (double)GetValue(TapZoomScaleProperty); }
            set { SetValue(TapZoomScaleProperty, value); }
        }

        public static readonly BindableProperty MaxZoomProperty =
            BindableProperty.Create(nameof(MaxZoom), typeof(double), typeof(ZoomImage), 10.0);

        public double MaxZoom
        {
            get { return (double)GetValue(MaxZoomProperty); }
            set { SetValue(MaxZoomProperty, value); }
        }

        public static readonly BindableProperty MinZoomProperty =
            BindableProperty.Create(nameof(MinZoom), typeof(double), typeof(ZoomImage), 1.0);

        public double MinZoom
        {
            get { return (double)GetValue(MinZoomProperty); }
            set { SetValue(MinZoomProperty, value); }
        }

        public static readonly BindableProperty CurrentZoomProperty =
            BindableProperty.Create(nameof(CurrentZoom), typeof(double), typeof(ZoomImage), 1.0);

        public double CurrentZoom
        {
            get { return (double)GetValue(CurrentZoomProperty); }
            set { SetValue(CurrentZoomProperty, value); }
        }
    }
}