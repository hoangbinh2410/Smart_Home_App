using BA_MobileGPS.Core.DependencyServices;
using BA_MobileGPS.Utilities.Enums;
using Prism.Ioc;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(BA_MobileGPS.Core.iOS.CustomRenderer.PageRenderer))]

namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetAppTheme();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            if (this.TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetAppTheme();
            }
        }

        private void SetAppTheme()
        {
            var themeService = Prism.PrismApplicationBase.Current.Container.Resolve<IThemeService>();
            if (themeService != null)
            {
                if (TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
                {
                    themeService.UpdateTheme(ThemeMode.Dark);
                }
                else
                {
                    themeService.UpdateTheme(ThemeMode.Light);
                }
            }
        }
    }
}