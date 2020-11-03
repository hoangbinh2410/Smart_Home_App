using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.iOS.CustomRenderer;
using CoreGraphics;
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
        private nfloat centerX;
        private nfloat centerY;
        IPageController PageController => Element as IPageController;
        public bool IsTabBarVisible
        {
            get
            {
                return !TabBar.Hidden;
            }

            set
            {
                TabBar.Hidden = !value;
            }
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement as TabbedPageEx != null)
            {
                var tabbedPage = e.NewElement as TabbedPageEx;
                if (tabbedPage != null)
                {
                    centerX = TabBar.Center.X;
                    centerY = TabBar.Center.Y;
                }
            }
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

        private void OnTabBarHidden(bool isHidden)
        {
            if (this.disposed || this.Element == null || this.TabBar == null)
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
            if (TabBar.Hidden)
                return;
            var frame = View.Frame;
            var tabBarFrame = TabBar.Frame;
            PageController.ContainerArea =
                new Rectangle(0, 0, frame.Width, frame.Height - tabBarFrame.Height);
            var animationOptions = UIViewAnimationOptions.BeginFromCurrentState |
                                   UIViewAnimationOptions.CurveEaseInOut;

            Action noOpOnCompletion = () => { };

            Action slideUpAction = () =>
            {
                TabBar.LayoutIfNeeded();
                TabBar.Center = new CGPoint(centerX, centerY);
                TabBar.LayoutIfNeeded();
            };

            UIView.Animate(0.5, 0.0, animationOptions, slideUpAction, noOpOnCompletion);
        }

        public void SlideDown()
        {
            if (TabBar.Hidden)
                return;
            var frame = View.Frame;
            PageController.ContainerArea =
                   new Rectangle(0, 0, frame.Width, frame.Height);
            var animationOptions = UIViewAnimationOptions.BeginFromCurrentState |
                                   UIViewAnimationOptions.CurveEaseInOut;

            Action noOpOnCompletion = () => { };

            Action slideDownAction = () =>
            {
                var newFrame = TabBar.Frame;
                newFrame.Offset(0, TabBar.Frame.Size.Height);
                TabBar.Frame = newFrame;
            };

            UIView.Animate(0.5, 0.0, animationOptions, slideDownAction, noOpOnCompletion);
        }
    }
}