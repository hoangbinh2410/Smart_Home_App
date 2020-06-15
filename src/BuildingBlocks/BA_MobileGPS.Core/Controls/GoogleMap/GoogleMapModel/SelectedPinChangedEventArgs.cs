using System;

namespace BA_MobileGPS.Core
{
    public sealed class SelectedPinChangedEventArgs : EventArgs
    {
        public Pin SelectedPin
        {
            get;
            private set;
        }

        public SelectedPinChangedEventArgs(Pin selectedPin)
        {
            this.SelectedPin = selectedPin;
        }
    }
}