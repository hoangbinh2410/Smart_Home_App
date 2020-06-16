using Android.Views;
using BA_MobileGPS.Core.Droid.DependencyServices;
using Com.Tomergoldst.Tooltips;

using Plugin.CurrentActivity;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using static Com.Tomergoldst.Tooltips.ToolTipsManager;

[assembly: Dependency(typeof(DroidTooltipService))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    /// <summary>
    /// namth custom ẩn hiện tooltip
    /// </summary>
    public class DroidTooltipService : ITooltipService
    {
        private ToolTip toolTipView;
        private ToolTipsManager _toolTipsManager;
        private ITipListener listener;

        public DroidTooltipService()
        {
            listener = new TipListener();
            _toolTipsManager = new ToolTipsManager(listener);
        }

        public void HideTooltip(Xamarin.Forms.View onView)
        {
            _toolTipsManager.FindAndDismiss(GetOrCreateRenderer(onView).View);
        }

        public void ShowTooltip(Xamarin.Forms.View onView, TooltipConfig config)
        {
            var control = GetOrCreateRenderer(onView).View;

            if (!string.IsNullOrEmpty(config.Text))
            {
                ToolTip.Builder builder;
                var position = config.Position;
                var parentContent = control.RootView;
                switch (position)
                {
                    case TooltipPosition.Top:
                        builder = new ToolTip.Builder(global::Android.App.Application.Context, control,
                            parentContent as ViewGroup, config.Text.PadRight(80, ' '), ToolTip.PositionAbove);
                        break;

                    case TooltipPosition.Left:
                        builder = new ToolTip.Builder(global::Android.App.Application.Context, control,
                            parentContent as ViewGroup, config.Text.PadRight(80, ' '), ToolTip.PositionLeftTo);
                        break;

                    case TooltipPosition.Right:
                        builder = new ToolTip.Builder(global::Android.App.Application.Context, control,
                            parentContent as ViewGroup, config.Text.PadRight(80, ' '), ToolTip.PositionRightTo);
                        break;

                    default:
                        builder = new ToolTip.Builder(global::Android.App.Application.Context, control,
                            parentContent as ViewGroup, config.Text.PadRight(80, ' '), ToolTip.PositionBelow);
                        break;
                }

                builder.SetAlign(ToolTip.AlignLeft);
                builder.SetBackgroundColor(config.BackgroundColor.ToAndroid());
                builder.SetTextColor(config.TextColor.ToAndroid());

                toolTipView = builder.Build();

                _toolTipsManager?.Show(toolTipView);
            }
        }

        private static IVisualElementRenderer GetOrCreateRenderer(VisualElement element)
        {
            var renderer = Platform.GetRenderer(element);
            if (renderer == null)
            {
                renderer = Platform.CreateRendererWithContext(element, CrossCurrentActivity.Current.Activity);
                Platform.SetRenderer(element, renderer);
            }
            return renderer;
        }
    }

    internal class TipListener : Java.Lang.Object, ITipListener
    {
        public void OnTipDismissed(Android.Views.View p0, int p1, bool p2)
        {
        }
    }
}