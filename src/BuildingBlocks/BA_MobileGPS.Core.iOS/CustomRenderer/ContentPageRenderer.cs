using BA_MobileGPS.Core.iOS.CustomRenderer;

using System.Linq;
using System.Reflection;

using UIKit;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ContentPage), typeof(ContentPageRenderer))]

namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class ContentPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            //try
            //{
            //    SetAppTheme();
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            //}
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // Set the status bar to light.
            //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            var contentPage = this.Element as ContentPage;
            if (contentPage == null || NavigationController == null)
                return;

            var itemsInfo = contentPage.ToolbarItems;

            // Move toolbaritems over to the left if we have more than 1.
            var navigationItem = this.NavigationController.TopViewController.NavigationItem;
            var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
            var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

            var newLeftButtons = new UIBarButtonItem[] { }.ToList();
            var newRightButtons = new UIBarButtonItem[] { }.ToList();

            rightNativeButtons.ForEach(nativeItem =>
            {
                // [Hack] Get Xamarin private field "item"
                var field = nativeItem.GetType().GetField("_item", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)
                    return;

                var info = field.GetValue(nativeItem) as ToolbarItem;
                if (info == null)
                    return;

                if (info.Priority == 0)
                    newLeftButtons.Add(nativeItem);
                else
                    newRightButtons.Add(nativeItem);
            });

            leftNativeButtons.ForEach(nativeItem =>
            {
                newLeftButtons.Add(nativeItem);
            });

            navigationItem.RightBarButtonItems = newRightButtons.ToArray();
            navigationItem.LeftBarButtonItems = newLeftButtons.ToArray();

            ModalPresentationCapturesStatusBarAppearance = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            // Set the status bar to light.
            //UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            //if (this.TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            //{
            //    SetAppTheme();
            //}
        }
        //private void SetAppTheme()
        //{
        //    var themeService = Prism.PrismApplicationBase.Current.Container.Resolve<IThemeService>();
        //    if (themeService != null)
        //    {
        //        if (TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
        //        {
        //            themeService.UpdateTheme(OSAppTheme.Dark);
        //        }
        //        else
        //        {
        //            themeService.UpdateTheme(OSAppTheme.Light);
        //        }
        //    }
        //}
    }
}