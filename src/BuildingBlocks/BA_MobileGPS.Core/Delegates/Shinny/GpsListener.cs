using Shiny.Locations;
using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Delegates.Shinny
{
    public class GpsListener : IGpsListener
    {
        public event EventHandler<GpsReadingEventArgs> OnReadingReceived;

        private void UpdateReading(IGpsReading reading)
        {
            OnReadingReceived?.Invoke(this, new GpsReadingEventArgs(reading));
        }

        public class LocationDelegate : IGpsDelegate
        {
            private IGpsListener _gpsListener;

            public LocationDelegate(IGpsListener gpsListener)
            {
                _gpsListener = gpsListener;
            }

            public Task OnReading(IGpsReading reading)
            {
                (_gpsListener as GpsListener)?.UpdateReading(reading);
                return Task.CompletedTask;
            }
        }
    }
}