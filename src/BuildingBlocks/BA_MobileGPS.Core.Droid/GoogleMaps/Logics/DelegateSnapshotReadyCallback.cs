using Android.Graphics;

using System;

namespace BA_MobileGPS.Core.Droid.Logics
{
    public sealed class DelegateSnapshotReadyCallback : Java.Lang.Object, Android.Gms.Maps.GoogleMap.ISnapshotReadyCallback
    {
        private readonly Action<Bitmap> _handler;

        public DelegateSnapshotReadyCallback(Action<Bitmap> handler)
        {
            _handler = handler;
        }

        public void OnSnapshotReady(Bitmap snapshot)
        {
            _handler?.Invoke(snapshot);
        }
    }
}