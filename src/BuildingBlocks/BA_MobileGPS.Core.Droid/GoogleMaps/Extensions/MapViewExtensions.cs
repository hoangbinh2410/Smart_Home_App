using Android.Gms.Maps;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Droid.Extensions
{
    public static class MapViewExtensions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Task<Android.Gms.Maps.GoogleMap> GetGoogleMapAsync(this MapView self)
        {
            var comp = new TaskCompletionSource<Android.Gms.Maps.GoogleMap>();
            self.GetMapAsync(new OnMapReadyCallback(map =>
            {
                comp.SetResult(map);
            }));

            return comp.Task;
        }
    }

    public class OnMapReadyCallback : Java.Lang.Object, IOnMapReadyCallback
    {
        private readonly Action<Android.Gms.Maps.GoogleMap> handler;

        public OnMapReadyCallback(Action<Android.Gms.Maps.GoogleMap> handler)
        {
            this.handler = handler;
        }

        void IOnMapReadyCallback.OnMapReady(Android.Gms.Maps.GoogleMap googleMap)
        {
            handler?.Invoke(googleMap);
        }
    }
}