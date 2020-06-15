using System;

namespace BA_MobileGPS.Core
{
    public sealed class PinDragEventArgs : EventArgs
    {
        public Pin Pin
        {
            get;
            private set;
        }

        public PinDragEventArgs(Pin pin)
        {
            this.Pin = pin;
        }
    }
}