using System;

namespace BA_MobileGPS.Core
{
    public sealed class MyLocationButtonClickedEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;

        public MyLocationButtonClickedEventArgs()
        {
        }
    }
}