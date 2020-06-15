using System;

namespace BA_MobileGPS.Core
{
    public sealed class InfoWindowClickedEventArgs : EventArgs
    {
        public Pin Pin { get; }

        public InfoWindowClickedEventArgs(Pin pin)
        {
            this.Pin = pin;
        }
    }
}