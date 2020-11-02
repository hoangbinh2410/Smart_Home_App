using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.iOS.CustomRenderer;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPageEx), typeof(TabbedPageExRenderer))]
namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    
    public class TabbedPageExRenderer : TabbedRenderer
    {
        private bool disposed;
        private const int TabBarHeight = 49;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                this.Tabbed.PropertyChanged += Tabbed_PropertyChanged;
            }
        }

        private void Tabbed_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TabbedPageEx.IsHiddenProperty.PropertyName)
            {
                this.OnTabBarHidden((this.Element as TabbedPageEx).IsHidden);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.disposed = true;
        }

        private async void OnTabBarHidden(bool isHidden)
        {
            if (this.disposed || this.Element == null || this.TabBar == null)
            {
                return;
            }

            await this.SetTabBarVisibility(isHidden);
        }

        private async Task SetTabBarVisibility(bool hide)
        {
            this.TabBar.Opaque = false;
            if (hide)
            {
                this.TabBar.Alpha = 0;
            }

            this.UpdateFrame(hide);

            // Show / Hide TabBar
            this.TabBar.Hidden = hide;
            this.RestoreFonts();

            // Animate appearing 
            if (!hide)
            {
                await UIView.AnimateAsync(0.2f, () => this.TabBar.Alpha = 1);
            }
            this.TabBar.Opaque = true;

            this.ResizeViewControllers();
            this.RestoreFonts();
        }

        private void UpdateFrame(bool isHidden)
        {
            var tabFrame = this.TabBar.Frame;
            tabFrame.Height = isHidden ? 0 : TabBarHeight;
            this.TabBar.Frame = tabFrame;
        }

        private void RestoreFonts()
        {
            // Workaround to restore custom fonts:

            foreach (var item in this.TabBar.Items)
            {
                var text = item.Title;
                item.Title = "";
                item.Title = text;
            }
        }

        private void ResizeViewControllers()
        {
            foreach (var child in this.ChildViewControllers)
            {
                child.View.SetNeedsLayout();
                child.View.SetNeedsDisplay();
            }
        }
    }
}