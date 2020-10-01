using BA_MobileGPS.Core.iOS.DependencyServices;
using GlobalToast;
using GlobalToast.Animation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DisplayMessageService))]

namespace BA_MobileGPS.Core.iOS.DependencyServices
{
    /// <summary>
    /// Class dùng chung để hiển thị thông báo
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// namth  12/7/2018   created
    /// </Modified>
    /// <Modified>
    /// Name     Date         Comments
    /// namth  12/7/2018   created
    /// </Modified>
    public class DisplayMessageService : IDisplayMessage
    {
        public void ShowMessageError(string message, double time)
        {
            // More configurations
            Toast.MakeToast(message)
                 .SetPosition(ToastPosition.Bottom) // Default is Bottom
                 .SetDuration(time) // Default is Regular
                 .SetShowShadow(false) // Default is true
                 .SetAnimator(new ScaleAnimator()) // Default is FadeAnimator
                 .SetBlockTouches(false) // Default is false. If false touches that occur on the toast will be sent down to parent view
                 .SetAutoDismiss(true) // Default is true. If set to false Dismiss button will be shown
                 .SetDismissButtonTitle("X")
                 .SetParentController(null)
                 .SetAppearance(new ToastAppearance
                 {
                     Color = UIColor.Red,
                     CornerRadius = 8,
                     DismissButtonColor = UIColor.White
                     // Other properties removed for brevity
                 }).SetLayout(new ToastLayout

                 {
                     PaddingLeading = 16,
                     PaddingTrailing = 16,
                     Spacing = 6,
                     // Other properties removed for brevity
                 })
                 .Show();
        }

        public void ShowMessageInfo(string message, double time)
        {
            // More configurations
            Toast.MakeToast(message)
                 .SetPosition(ToastPosition.Bottom) // Default is Bottom
                 .SetDuration(time) // Default is Regular
                 .SetShowShadow(false) // Default is true
                 .SetAnimator(new ScaleAnimator()) // Default is FadeAnimator
                 .SetBlockTouches(false) // Default is false. If false touches that occur on the toast will be sent down to parent view
                 .SetAutoDismiss(true) // Default is true. If set to false Dismiss button will be shown
                 .SetDismissButtonTitle("X")
                 .SetParentController(null)
                 .SetAppearance(new ToastAppearance
                 {
                     Color = UIColor.Black,
                     CornerRadius = 8,
                     DismissButtonColor = UIColor.White
                     // Other properties removed for brevity
                 }).SetLayout(new ToastLayout
                 {
                     PaddingLeading = 16,
                     PaddingTrailing = 16,
                     Spacing = 6,
                     // Other properties removed for brevity
                 })
                 .Show();
        }

        public void ShowMessageWarning(string message, double time)
        {
            // More configurations
            Toast.MakeToast(message)
                 .SetPosition(ToastPosition.Bottom) // Default is Bottom
                 .SetDuration(time) // Default is Regular
                 .SetShowShadow(false) // Default is true
                 .SetAnimator(new ScaleAnimator()) // Default is FadeAnimator
                 .SetBlockTouches(false) // Default is false. If false touches that occur on the toast will be sent down to parent view
                 .SetAutoDismiss(true) // Default is true. If set to false Dismiss button will be shown
                 .SetDismissButtonTitle("X")
                 .SetParentController(null)
                 .SetAppearance(new ToastAppearance
                 {
                     Color = UIColor.Orange,
                     CornerRadius = 8,
                     DismissButtonColor = UIColor.White
                     // Other properties removed for brevity
                 }).SetLayout(new ToastLayout
                 {
                     PaddingLeading = 16,
                     PaddingTrailing = 16,
                     Spacing = 6,
                     // Other properties removed for brevity
                 })
                 .Show();
        }

        public void ShowMessageSuccess(string message, double time)
        {
            // More configurations
            Toast.MakeToast(message)
                 .SetPosition(ToastPosition.Bottom) // Default is Bottom
                 .SetDuration(time) // Default is Regular
                 .SetShowShadow(false) // Default is true
                 .SetAnimator(new ScaleAnimator()) // Default is FadeAnimator
                 .SetBlockTouches(false) // Default is false. If false touches that occur on the toast will be sent down to parent view
                 .SetAutoDismiss(true) // Default is true. If set to false Dismiss button will be shown
                 .SetDismissButtonTitle("X")
                 .SetParentController(null)
                 .SetAppearance(new ToastAppearance
                 {
                     Color = UIColor.Blue,
                     CornerRadius = 8,
                     DismissButtonColor = UIColor.White
                     // Other properties removed for brevity
                 }).SetLayout(new ToastLayout
                 {
                     PaddingLeading = 16,
                     PaddingTrailing = 16,
                     Spacing = 6,
                     // Other properties removed for brevity
                 })
                 .Show();
        }

        public void ShowToast(string message, double time = 5000)
        {
            // More configurations
            Toast.MakeToast(message)
                 .SetPosition(ToastPosition.Bottom) // Default is Bottom
                 .SetDuration(time) // Default is Regular
                 .SetShowShadow(false) // Default is true
                 .SetAnimator(new ScaleAnimator()) // Default is FadeAnimator
                 .SetBlockTouches(false) // Default is false. If false touches that occur on the toast will be sent down to parent view
                 .SetAutoDismiss(true) // Default is true. If set to false Dismiss button will be shown
                 .SetDismissButtonTitle("X")
                 .SetParentController(null)
                 .SetAppearance(new ToastAppearance
                 {
                     Color = UIColor.Black,
                     CornerRadius = 8,
                     DismissButtonColor = UIColor.White
                     // Other properties removed for brevity
                 }).SetLayout(new ToastLayout
                 {
                     PaddingLeading = 16,
                     PaddingTrailing = 16,
                     Spacing = 6,
                     // Other properties removed for brevity
                 })
                 .Show();
        }
    }
}