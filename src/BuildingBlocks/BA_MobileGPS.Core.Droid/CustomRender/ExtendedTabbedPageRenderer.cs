using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.Design.Internal;
using Android.Graphics;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Android.Views;

using Android.Widget;
using Android.Graphics.Drawables;
using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.Droid.CustomRender;
using Android.OS;

[assembly: ExportRenderer(typeof(ExtendedTabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace BA_MobileGPS.Core.Droid.CustomRender
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
        Xamarin.Forms.TabbedPage tabbedPage;
        BottomNavigationView bottomNavigationView;
        IMenuItem lastItemSelected;
        public ExtendedTabbedPageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                tabbedPage = e.NewElement as ExtendedTabbedPage;
                bottomNavigationView = (GetChildAt(0) as Android.Widget.RelativeLayout).GetChildAt(1) as BottomNavigationView;
                bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;

                //Call to remove animation
                SetShiftMode(bottomNavigationView, false);

            }

            if (e.OldElement != null)
            {
                bottomNavigationView.NavigationItemSelected -= BottomNavigationView_NavigationItemSelected;
            }


        }



        //Remove tint color
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (bottomNavigationView != null)
            {
                bottomNavigationView.ItemIconTintList = null;
            }
        }

        void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            try
            {
                var normalColor = tabbedPage.UnselectedTabColor.ToAndroid();
                var selectedColor = tabbedPage.SelectedTabColor.ToAndroid();

                if (lastItemSelected != null)
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        lastItemSelected.Icon.SetColorFilter(new BlendModeColorFilter(normalColor, BlendMode.SrcIn));
                    }
                    else
                    {
                        lastItemSelected.Icon.SetColorFilter(normalColor, PorterDuff.Mode.SrcIn);
                    }
                }

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    e.Item.Icon.SetColorFilter(new BlendModeColorFilter(selectedColor, BlendMode.SrcIn));
                }
                else
                {
                    e.Item.Icon.SetColorFilter(selectedColor, PorterDuff.Mode.SrcIn);
                }
                lastItemSelected = e.Item;

                this.OnNavigationItemSelected(e.Item);
            }
            catch (Exception ex)
            {

               
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