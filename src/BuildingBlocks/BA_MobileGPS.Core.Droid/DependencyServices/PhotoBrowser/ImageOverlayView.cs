using Android.Content;
using Android.Views;
using Android.Widget;

using Com.Stfalcon.Frescoimageviewer;

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class ImageOverlayView : RelativeLayout, ImageViewer.IOnImageChangeListener
    {
        protected TextView tvDescription;

        protected TextView tvInfo;

        protected ImageButton btnAction;

        protected PhotoBrowser _photoBrowser;

        protected int _currentIndex = 0;

        public ImageOverlayView(Context context, PhotoBrowser photoBrowser) : base(context)
        {
            _photoBrowser = photoBrowser;
            init();
        }

        protected void init()
        {
            View view = Inflate(Context, 2131427438, this);
            tvDescription = view.FindViewById<TextView>(2131231189);
            tvInfo = view.FindViewById<TextView>(2131231190);
            btnAction = view.FindViewById<ImageButton>(2131230835);

            if (_photoBrowser.ActionButtonPressed != null)
            {
                btnAction.Click += (o, e) =>
                {
                    _photoBrowser.ActionButtonPressed?.Invoke(_currentIndex);
                };
            }
            else
            {
                btnAction.Visibility = ViewStates.Gone;
            }
        }

        public void OnImageChange(int p0)
        {
            tvInfo.Text = _photoBrowser.Photos[p0].Info;
            tvDescription.Text = _photoBrowser.Photos[p0].Title;
            _currentIndex = p0;

            _photoBrowser.DidDisplayPhoto?.Invoke(p0);
        }
    }
}