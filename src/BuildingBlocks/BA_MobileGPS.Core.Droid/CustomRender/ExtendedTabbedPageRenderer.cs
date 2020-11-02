using Android.Animation;
using Android.Content;
using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.Droid.CustomRender;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPageEx), typeof(ExtendedTabbedPageRenderer))]

namespace BA_MobileGPS.Core.Droid.CustomRender
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        BottomNavigationView bottomNavigationView;
        private bool slidingUp;
        private bool slidingDown;

        public bool SlidingDown
        {
            get
            {
                return slidingDown;
            }

            set
            {
                slidingDown = value;
            }
        }

        public bool SlidingUp
        {
            get
            {
                return slidingUp;
            }

            set
            {
                slidingUp = value;
            }
        }
        private int TabBarHeight;
        public BottomNavigationView BottomNavigationView
        {
            get
            {
                return bottomNavigationView;
            }

            private set
            {
                bottomNavigationView = value;
            }
        }
        public ExtendedTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (BottomNavigationView == null)
                {
                    bottomNavigationView = (GetChildAt(0) as Android.Widget.RelativeLayout).GetChildAt(1) as BottomNavigationView;
                }
            }


        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == TabbedPageEx.IsHiddenProperty.PropertyName)
            {
                for (int i = 0; i <= this.ViewGroup.ChildCount - 1; i++)
                {
                    var childView = this.ViewGroup.GetChildAt(i);
                    if (childView is ViewGroup viewGroup)
                    {
                        for (int j = 0; j <= viewGroup.ChildCount - 1; j++)
                        {
                            var childRelativeLayoutView = viewGroup.GetChildAt(j);
                            if (childRelativeLayoutView is BottomNavigationView)
                            {
                                if (((BottomNavigationView)childRelativeLayoutView).LayoutParameters.Height != 0) TabBarHeight = ((BottomNavigationView)childRelativeLayoutView).LayoutParameters.Height;

                                var parameters = ((BottomNavigationView)childRelativeLayoutView).LayoutParameters;
                                var hidden = (this.Element as TabbedPageEx).IsHidden;
                                this.OnTabBarHidden(hidden);
                                parameters.Height = hidden ? 0 : TabBarHeight;
                                ((BottomNavigationView)childRelativeLayoutView).LayoutParameters = parameters;
                            }
                        }
                    }
                }

            }
        }

        private void OnTabBarHidden(bool isHidden)
        {
            if (this.Element == null)
            {
                return;
            }

            if (isHidden)
            {
                SlideDown();
            }
            else
            {
                SlideUp();
            }
        }
        public void SlideUp()
        {
            var currentY = BottomNavigationView.TranslationY;

            if (currentY == 0 || slidingUp)
                return;

            bottomNavigationView.Animate().Cancel();

            SlidingUp = true;

            bottomNavigationView
                .Animate()
                .TranslationY(0)
                .SetDuration(250)
                .SetListener(new BottomTabAnimationListener(this))
                .Start();
        }

        public void SlideDown()
        {
            var navigationBarHeight = BottomNavigationView.Height;
            var currentY = BottomNavigationView.TranslationY;

            if (currentY == navigationBarHeight || slidingDown)
                return;

            bottomNavigationView.Animate().Cancel();

            SlidingDown = true;

            bottomNavigationView
                .Animate()
                .TranslationY(navigationBarHeight)
                .SetDuration(250)
                .SetListener(new BottomTabAnimationListener(this))
                .Start();
        }
    }
    class BottomTabAnimationListener : Java.Lang.Object, Animator.IAnimatorListener
    {
        ExtendedTabbedPageRenderer renderer;

        public BottomTabAnimationListener(ExtendedTabbedPageRenderer renderer)
        {
            this.renderer = renderer;
        }
        public void OnAnimationCancel(Animator animation)
        {
            UpdateFlags();
        }

        public void OnAnimationEnd(Animator animation)
        {
            UpdateFlags();
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }

        private void UpdateFlags()
        {
            if (renderer.SlidingUp)
                renderer.SlidingUp = false;

            if (renderer.SlidingDown)
                renderer.SlidingDown = false;
        }
    }
}