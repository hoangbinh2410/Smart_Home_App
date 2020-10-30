using Android.Content;
using Android.Support.Design.BottomNavigation;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using BA_MobileGPS.Core.Droid.CustomRender;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(Xamarin.Forms.TabbedPage), typeof(ExtendedTabbedPageRenderer))]

namespace BA_MobileGPS.Core.Droid.CustomRender
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
        private Xamarin.Forms.TabbedPage tabbedPage;
        private BottomNavigationView bottomNavigationView;

        public ExtendedTabbedPageRenderer(Context context) : base(context)
        {
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