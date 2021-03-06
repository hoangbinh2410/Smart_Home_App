using BA_MobileGPS.Core.Controls;
using BA_MobileGPS.Core.iOS.CustomRenderer;

using CoreGraphics;

using System;
using System.ComponentModel;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(IconView), typeof(IconViewRenderer))]

namespace BA_MobileGPS.Core.iOS.CustomRenderer
{
    public class IconViewRenderer : ViewRenderer<IconView, UIImageView>
    {
        private bool _isDisposed;

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing && Control != null)
            {
                UIImage image = Control.Image;
                UIImage uIImage = image;
                if (image != null)
                {
                    uIImage.Dispose();
                    uIImage = null;
                }
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<IconView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                UIImageView uIImageView = new UIImageView(CGRect.Empty)
                {
                    ContentMode = UIViewContentMode.ScaleAspectFit,
                    ClipsToBounds = true
                };
                SetNativeControl(uIImageView);
            }
            if (e.NewElement != null)
            {
                SetImage(e.OldElement);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconView.SourceProperty.PropertyName)
            {
                SetImage(null);
            }
            else if (e.PropertyName == IconView.ForegroundProperty.PropertyName)
            {
                SetImage(null);
            }
        }

        private void SetImage(IconView previous = null)
        {
            try
            {
                if (previous == null)
                {
                    var uiImage = new UIImage(Element.Source);
                    if (uiImage != null)
                    {
                        uiImage = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                        Control.TintColor = Element.Foreground.ToUIColor();
                        Control.Image = uiImage;
                        if (!_isDisposed)
                        {
                            ((IVisualElementController)Element).NativeSizeChanged();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}