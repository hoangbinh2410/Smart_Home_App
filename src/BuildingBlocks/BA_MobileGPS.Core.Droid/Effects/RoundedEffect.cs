using Android.Graphics.Drawables;
using Android.Support.V4.Content;

using BA_MobileGPS.Core.Droid.Effects;

using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(RoundedEffect), "RoundedEffect")]

namespace BA_MobileGPS.Core.Droid.Effects
{
    public class RoundedEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Element is SearchBar searchBar)
                {
                    searchBar.HeightRequest = 55;

                    var plate = Control.FindViewById(Control.Resources.GetIdentifier("android:id/search_plate", null, null));
                    if (plate != null)
                        plate.Background = new ColorDrawable(Android.Graphics.Color.Transparent);

                    Control.Background = ContextCompat.GetDrawable(Android.App.Application.Context, Resource.Drawable.RoundedSearchBarBackground);
                    //Control.SetForegroundGravity(GravityFlags.CenterVertical);
                    Control.SetPadding(20, 14, 20, 16);

                    return;
                }

                Control.Background = ContextCompat.GetDrawable(Android.App.Application.Context, Resource.Drawable.RoundedBackground);
                //Control.SetForegroundGravity(GravityFlags.CenterVertical);
                Control.SetPadding(20, 14, 20, 16);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}