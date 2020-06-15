﻿using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace BA_MobileGPS.Core.Droid
{
    /**
 * IconGenerator generates icons that contain text (or custom content) within an info
 * window-like shape.
 * <p/>
 * The icon {@link Bitmap}s generated by the factory should be used in conjunction with a {@link
 * com.google.android.gms.maps.model.BitmapDescriptorFactory}.
 * <p/>
 * This class is not thread safe.
 */

    public class IconClusterGenerator
    {
        private Context mContext;

        private LinearLayout mContainer;
        private TextView mTextView;
        /**
         * Creates a new IconGenerator with the default style.
         */

        public IconClusterGenerator(Context context)
        {
            mContext = context;
            mContainer = (LinearLayout)LayoutInflater.From(mContext).Inflate(Resource.Layout.amu_text_bubble_cluster, null);
            mTextView = mTextView = (TextView)mContainer.FindViewById(Resource.Id.amu_text);
        }

        /**
         * Sets the text content, then creates an icon with the current style.
         *
         * @param text the text content to display inside the icon.
         */

        public Bitmap MakeIcon(string text)
        {
            if (mTextView != null)
            {
                mTextView.Text = text;
            }

            return MakeIcon();
        }

        /**
         * Creates an icon with the current content and style.
         * <p/>
         * This method is useful if a custom view has previously been set, or if text content is not
         * applicable.
         */

        public Bitmap MakeIcon()
        {
            int measureSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            mContainer.Measure(measureSpec, measureSpec);

            int measuredWidth = mContainer.MeasuredWidth;
            int measuredHeight = mContainer.MeasuredHeight;

            mContainer.Layout(0, 0, measuredWidth, measuredHeight);

            Bitmap r = Bitmap.CreateBitmap(measuredWidth, measuredHeight, Bitmap.Config.Argb8888);
            r.EraseColor(Color.Transparent);

            Canvas canvas = new Canvas(r);

            canvas.DrawColor(Color.White, PorterDuff.Mode.SrcIn);
            Drawable drawable = mContainer.Background;
            if (drawable != null)
                drawable.Draw(canvas);
            mContainer.Draw(canvas);
            return r;
        }

        public void SetBackground(Drawable background)
        {
            int sdk = (int)Build.VERSION.SdkInt;
            if (sdk < (int)Android.OS.BuildVersionCodes.JellyBean)
            {
                mTextView.SetBackgroundDrawable(background);
            }
            else
            {
                mTextView.Background = background;
            }

            // Force setting of padding.
            // setBackgroundDrawable does not call setPadding if the background has 0 padding.
            if (background != null)
            {
                Rect rect = new Rect();
                background.GetPadding(rect);
                mTextView.SetPadding(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
            else
            {
                mTextView.SetPadding(0, 0, 0, 0);
            }
        }
    }
}