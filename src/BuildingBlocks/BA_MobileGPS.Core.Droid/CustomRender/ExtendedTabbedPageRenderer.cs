using Android.Content;
using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
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
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
        private Xamarin.Forms.TabbedPage tabbedPage;
        private BottomNavigationView bottomNavigationView;
        private int TabBarHeight;

        public ExtendedTabbedPageRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "IsHidden")
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
                                parameters.Height = ((TabbedPageEx)sender).IsHidden ? 0 : TabBarHeight;

                                ((BottomNavigationView)childRelativeLayoutView).LayoutParameters = parameters;
                            }
                        }
                    }
                }
            }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                tabbedPage = e.NewElement as TabbedPage;
                bottomNavigationView = (GetChildAt(0) as Android.Widget.RelativeLayout).GetChildAt(1) as BottomNavigationView;
               
                //Call to remove animation
                SetShiftMode(bottomNavigationView, false);
                TabBarHeight = bottomNavigationView.LayoutParameters.Height;
            }
        }

        //Remove animation
        public void SetShiftMode(BottomNavigationView bottomNavigationView, bool enableItemShiftMode)
        {
            try
            {
                var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
                if (menuView == null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to find BottomNavigationMenuView");
                    return;
                }
                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                    if (item == null)
                        continue;
                    item.SetShifting(enableItemShiftMode);
                    item.SetLabelVisibilityMode(LabelVisibilityMode.LabelVisibilityLabeled);
                    item.SetChecked(item.ItemData.IsChecked);
                }
                menuView.UpdateMenuView();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to set shift mode: {ex}");
            }
        }
    }
}