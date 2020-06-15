using System;

namespace BA_MobileGPS.Core
{
    public sealed class CameraMoveStartedEventArgs : EventArgs
    {
        public bool IsGesture { get; }

        public CameraMoveStartedEventArgs(bool isGesture)
        {
            IsGesture = isGesture;
        }
    }
}