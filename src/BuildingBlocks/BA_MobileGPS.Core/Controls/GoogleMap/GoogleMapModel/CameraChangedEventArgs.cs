using System;

namespace BA_MobileGPS.Core
{
    public sealed class CameraChangedEventArgs : EventArgs
    {
        public CameraPosition Position
        {
            get;
        }

        public CameraChangedEventArgs(CameraPosition position)
        {
            Position = position;
        }
    }
}