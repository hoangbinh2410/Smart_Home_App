using BA_MobileGPS.Core.iOS.Factories;

using CoreGraphics;

using Foundation;

using Google.Maps.Utility;

using System;

using UIKit;

using Xamarin.Forms.Platform.iOS;

namespace BA_MobileGPS.Core.iOS
{
    public class ClusterIconGeneratorHandler : DefaultClusterIconGenerator
    {
        private readonly NSCache iconCache;
        private readonly ClusterOptions options;

        public ClusterIconGeneratorHandler(ClusterOptions options)
        {
            iconCache = new NSCache();
            this.options = options;
        }

        public override UIImage IconForSize(nuint size)
        {
            nuint bucketIndex = 0;
            var sizes = size / 2;
            if (options.EnableBuckets)
            {
                var buckets = options.Buckets;
                bucketIndex = BucketIndexForSize((nint)size / 2);
            }
            if (options.RendererCallback != null)
                return DefaultImageFactory.Instance.ToUIImage(options.RendererCallback(sizes.ToString()));
            if (options.RendererImage != null)
                return GetIconForText(sizes.ToString(), DefaultImageFactory.Instance.ToUIImage(options.RendererImage));
            return GetIconForText(sizes.ToString(), bucketIndex);
        }

        private nuint BucketIndexForSize(nint size)
        {
            uint index = 0;
            var buckets = options.Buckets;

            while (index + 1 < buckets.Length && buckets[index + 1] <= size)
                ++index;

            return index;
        }

        private UIImage GetIconForText(string text, UIImage baseImage)
        {
            var nsText = new NSString(text);
            var icon = iconCache.ObjectForKey(nsText);
            if (icon != null)
                return (UIImage)icon;

            var font = UIFont.BoldSystemFontOfSize(12);
            var size = baseImage.Size;
            UIGraphics.BeginImageContextWithOptions(size, false, 0.0f);
            baseImage.Draw(new CGRect(0, 0, size.Width, size.Height));
            var rect = new CGRect(0, 0, baseImage.Size.Width, baseImage.Size.Height);

            var paragraphStyle = NSParagraphStyle.Default;
            var attributes = new UIStringAttributes(NSDictionary.FromObjectsAndKeys(
                objects: new NSObject[] { font, paragraphStyle, options.RendererTextColor.ToUIColor() },
                keys: new NSObject[] { UIStringAttributeKey.Font, UIStringAttributeKey.ParagraphStyle, UIStringAttributeKey.ForegroundColor }
            ));

            var textSize = nsText.GetSizeUsingAttributes(attributes);
            var textRect = RectangleFExtensions.Inset(rect, (rect.Size.Width - textSize.Width) / 3,
                (rect.Size.Height - textSize.Height) / 1);
            nsText.DrawString(RectangleFExtensions.Integral(textRect), attributes);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            iconCache.SetObjectforKey(newImage, nsText);
            return newImage;
        }

        private UIImage GetIconForText(string text, nuint bucketIndex)
        {
            var nsText = new NSString(text);
            var icon = iconCache.ObjectForKey(nsText);
            if (icon != null)
                return (UIImage)icon;

            var font = UIFont.BoldSystemFontOfSize(14);
            var paragraphStyle = NSParagraphStyle.Default;
            var dict = NSDictionary.FromObjectsAndKeys(
                objects: new NSObject[] { font, paragraphStyle, options.RendererTextColor.ToUIColor() },
                keys: new NSObject[] { UIStringAttributeKey.Font, UIStringAttributeKey.ParagraphStyle, UIStringAttributeKey.ForegroundColor }
            );
            var attributes = new UIStringAttributes(dict);

            var textSize = nsText.GetSizeUsingAttributes(attributes);
            var rectDimension = Math.Max(20, Math.Max(textSize.Width, textSize.Height)) + 3 * bucketIndex + 6;
            var rect = new CGRect(0.0f, 0.0f, rectDimension, rectDimension);

            UIGraphics.BeginImageContext(rect.Size);
            UIGraphics.BeginImageContextWithOptions(rect.Size, false, 0);

            var ctx = UIGraphics.GetCurrentContext();
            ctx.SaveState();

            bucketIndex = (nuint)Math.Min((int)bucketIndex, options.BucketColors.Length - 1);
            var backColor = options.BucketColors[bucketIndex];
            ctx.SetFillColor(backColor.ToCGColor());
            ctx.FillEllipseInRect(rect);
            ctx.RestoreState();

            UIColor.White.SetColor();
            var textRect = rect.Inset((rect.Size.Width - textSize.Width) / 2,
                (rect.Size.Height - textSize.Height) / 2);
            nsText.DrawString(textRect.Integral(), attributes);

            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            iconCache.SetObjectforKey(newImage, nsText);
            return newImage;
        }

        private UIImage TextToImage(nuint size)
        {
            var text = size.ToString();
            var uiImage = UIImage.FromBundle("markClaster");
            nfloat fWidth = uiImage.Size.Width;
            nfloat fHeight = uiImage.Size.Height;

            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();

            using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)fWidth, (nint)fHeight, 8
                                                           , 4 * (nint)fWidth, CGColorSpace.CreateDeviceRGB()
                                                           , CGImageAlphaInfo.PremultipliedFirst))
            {
                ctx.DrawImage(new CGRect(0, 0, (double)fWidth, (double)fHeight), uiImage.CGImage);

                ctx.SelectFont("Roboto-Bold", 12, CGTextEncoding.MacRoman);

                //Measure the text's width - This involves drawing an invisible string to calculate the X position difference
                float start, end, textWidth;

                //Get the texts current position
                start = (float)ctx.TextPosition.X;
                //Set the drawing mode to invisible
                ctx.SetTextDrawingMode(CGTextDrawingMode.Invisible);
                //Draw the text at the current position
                ctx.ShowText(text);
                //Get the end position
                end = (float)ctx.TextPosition.X;
                //Subtract start from end to get the text's width
                textWidth = end - start;

                //Set the fill color to black. This is the text color.

                ctx.SetFillColor(UIColor.White.CGColor);

                //Set the drawing mode back to something that will actually draw Fill for example
                ctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

                //Draw the text at given coords.
                ctx.ShowTextAtPoint(uiImage.Size.Width / 2 - textWidth / 2, uiImage.Size.Height / 2, text);

                return UIImage.FromImage(ctx.ToImage());
            }
        }
    }
}