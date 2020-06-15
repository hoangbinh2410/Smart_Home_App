using System;

namespace BA_MobileGPS.Core
{
    public sealed class PinClickedEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
        public Pin Pin { get; }

        public PinClickedEventArgs(Pin pin)
        {
            this.Pin = pin;
        }
    }
}