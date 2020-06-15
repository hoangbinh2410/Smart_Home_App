﻿using Android.App;
using Android.Util;

namespace BA_MobileGPS.Core.Droid.Extensions
{
    public static class ActivityExtensions
    {
        public static float GetScaledDensity(this Activity self)
        {
            var metrics = new DisplayMetrics();
            self.WindowManager.DefaultDisplay.GetMetrics(metrics);
            return metrics.ScaledDensity;
        }
    }
}