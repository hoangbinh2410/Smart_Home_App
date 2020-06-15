using System;
using System.Collections.Generic;

namespace BA_MobileGPS.Core
{
    public sealed class ClusterClickedEventArgs : EventArgs
    {
        public bool Handled { get; set; } = true;
        public int ItemsCount { get; }
        public IEnumerable<Pin> Pins { get; }
        public Position Position { get; }

        internal ClusterClickedEventArgs(int itemsCount, IEnumerable<Pin> pins, Position position)
        {
            ItemsCount = itemsCount;
            Pins = pins;
            Position = position;
        }
    }
}