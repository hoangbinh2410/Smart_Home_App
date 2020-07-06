using BA_MobileGPS.Core.iOS.DependencyServices;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(iOSTooltipService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    public class iOSTooltipService : ITooltipService
    {
        private EasyTipView.EasyTipView tooltip;
        private UITapGestureRecognizer tapGestureRecognizer;

        public iOSTooltipService()
        {
            tooltip = new EasyTipView.EasyTipView();
            tooltip.DidDismiss += OnDismiss;
        }

        public void HideTooltip(View onView)
        {
            tooltip?.Dismiss();
        }

        public void ShowTooltip(View onView, TooltipConfig config)
        {
            var control = GetOrCreateRenderer(onView).NativeView;

            var text = config.Text;

            if (!string.IsNullOrEmpty(text))
            {
                tooltip.BubbleColor = config.BackgroundColor.ToUIColor();
                tooltip.ForegroundColor = config.TextColor.ToUIColor();
                tooltip.Text = new Foundation.NSString(text);
                var position = config.Position;
                switch (position)
                {
                    case TooltipPosition.Top:
                        tooltip.ArrowPosition = EasyTipView.ArrowPosition.Bottom;
                        break;

                    case TooltipPosition.Left:
                        tooltip.ArrowPosition = EasyTipView.ArrowPosition.Right;
                        break;

                    case TooltipPosition.Right:
                        tooltip.ArrowPosition = EasyTipView.ArrowPosition.Left;
                        break;

                    default:
                        tooltip.ArrowPosition = EasyTipView.ArrowPosition.Top;
                        break;
                }
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                tooltip?.Show(control, vc.View, true);
            }
        }

        private void OnDismiss(object sender, EventArgs e)
        {
            // do something on dismiss
        }

        public static IVisualElementRenderer GetOrCreateRenderer(VisualElement element)
        {
            var renderer = Platform.GetRenderer(element);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(element);
                Platform.SetRenderer(element, renderer);
            }
            return renderer;
        }
    }
}