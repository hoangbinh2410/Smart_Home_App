﻿// Original code from https://github.com/javiholcman/Wapps.Forms.Map/
// Cacheing implemented by Gadzair

using CoreGraphics;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace BA_MobileGPS.Core.iOS
{
    public static class Utils
    {
        public static UIView ConvertFormsToNative(View view, CGRect size)
        {
            var renderer = Platform.CreateRenderer(view);

            renderer.NativeView.Frame = size;

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;

            renderer.Element.Layout(size.ToRectangle());

            var nativeView = renderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;
        }

        private static LinkedList<string> lruTracker = new LinkedList<string>();

        public static UIImage ConvertViewToImage(Pin vehicle)
        {
            UIImage img = new UIImage();
            var iconView = vehicle.Icon.View;
            var nativeView = Utils.ConvertFormsToNative(iconView, new CGRect(0, 0, iconView.WidthRequest, iconView.HeightRequest));
            nativeView.BackgroundColor = iconView.BackgroundColor.ToUIColor();
            img = nativeView.AsImage();
            return img;
        }

        private static ConcurrentDictionary<string, UIImage> cacheGround = new ConcurrentDictionary<string, UIImage>();

        public static UIImage ConvertViewToImage(UIView view)
        {
            UIGraphics.BeginImageContextWithOptions(view.Bounds.Size, false, 0);
            view.Layer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            // Optimization: Let's try to reuse any of the last 10 images we generated
            var bytes = img.AsPNG().ToArray();
            var md5 = MD5.Create();
            var hash = Convert.ToBase64String(md5.ComputeHash(bytes));

            var exists = cacheGround.ContainsKey(hash);
            if (exists)
            {
                lruTracker.Remove(hash);
                lruTracker.AddLast(hash);
                return cacheGround[hash];
            }
            if (lruTracker.Count > 10) // O(1)
            {
                UIImage tmp;
                cacheGround.TryRemove(lruTracker.First.Value, out tmp);
                lruTracker.RemoveFirst();
            }
            cacheGround.GetOrAdd(hash, img);
            lruTracker.AddLast(hash);
            return img;
        }

        public static UIImage AsImage(this UIView view)

        {
            UIGraphics.BeginImageContextWithOptions(view.Bounds.Size, true, 0);

            view.Layer.RenderInContext(UIGraphics.GetCurrentContext());

            var img = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return img;
        }
    }
}