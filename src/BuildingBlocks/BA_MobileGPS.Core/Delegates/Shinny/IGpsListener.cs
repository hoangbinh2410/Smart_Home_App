using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Delegates.Shinny
{
    public interface IGpsListener
    {
        event EventHandler<GpsReadingEventArgs> OnReadingReceived;
    }
    public class GpsReadingEventArgs : EventArgs
    {
        public IGpsReading Reading { get; }

        public GpsReadingEventArgs(IGpsReading reading)
        {
            Reading = reading;
        }

    }
}
