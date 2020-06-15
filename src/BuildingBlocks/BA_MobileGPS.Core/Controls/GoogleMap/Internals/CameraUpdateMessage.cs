﻿using System;

namespace BA_MobileGPS.Core.Internals
{
    public sealed class CameraUpdateMessage
    {
        public CameraUpdate Update { get; }
        public TimeSpan? Duration { get; }
        public IAnimationCallback Callback { get; }

        public CameraUpdateMessage(CameraUpdate update, TimeSpan? duration, IAnimationCallback callback)
        {
            Update = update;
            Duration = duration;
            Callback = callback;
        }
    }
}