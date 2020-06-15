using System;

namespace BA_MobileGPS.Core
{
    public sealed class InfoWindowLongClickedEventArgs : EventArgs
    {
        public Pin Pin { get; }

        public InfoWindowLongClickedEventArgs(Pin pin)
        {
            this.Pin = pin;
        }
    }
}