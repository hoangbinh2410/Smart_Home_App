using System;

namespace BA_MobileGPS.Core
{
    public sealed class MapLongClickedEventArgs : EventArgs
    {
        public Position Point { get; }

        public MapLongClickedEventArgs(Position point)
        {
            this.Point = point;
        }
    }
}