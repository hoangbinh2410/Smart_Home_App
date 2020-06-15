using System;

namespace BA_MobileGPS.Core
{
    public sealed class CameraIdledEventArgs : EventArgs
    {
        public CameraPosition Position { get; }

        public CameraIdledEventArgs(CameraPosition position)
        {
            this.Position = position;
        }
    }
}