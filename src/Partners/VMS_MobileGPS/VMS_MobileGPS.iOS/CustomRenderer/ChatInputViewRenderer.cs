using CoreGraphics;

using Foundation;

using UIKit;

using VMS_MobileGPS.iOS.Renderers;
using VMS_MobileGPS.Views;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ChatInputView), typeof(ChatInputViewRenderer))]

namespace VMS_MobileGPS.iOS.Renderers
{
    public class ChatInputViewRenderer : ViewRenderer
    {
        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        private void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            NSNumber animationDuration = (NSValue)args.Notification.UserInfo.ValueForKey(UIKeyboard.AnimationDurationUserInfoKey) as NSNumber;
            CGSize keyboardSize = result.RectangleFValue.Size;
            if (Element != null)
            {
                UIView.Animate(animationDuration.FloatValue, () =>
                {
                    Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height); //push the entry up to keyboard height when keyboard is activated
                }, completion: null);
            }
        }

        private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                NSNumber animationDuration = (NSValue)args.Notification.UserInfo.ValueForKey(UIKeyboard.AnimationDurationUserInfoKey) as NSNumber;
                UIView.Animate(animationDuration.FloatValue, () =>
                {
                    Element.Margin = new Thickness(0); //set the margins to zero when keyboard is dismissed
                }, completion: null);
            }
        }

        private void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }
    }
}