using System;

namespace BA_MobileGPS.Core
{
    public sealed class MapClickedEventArgs : EventArgs
    {
        public Position Point { get; }

        public MapClickedEventArgs(Position point)
        {
            this.Point = point;
        }
    }
}