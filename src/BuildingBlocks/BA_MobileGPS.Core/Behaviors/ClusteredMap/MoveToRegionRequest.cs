using System;

namespace BA_MobileGPS.Core.Behaviors.ClusteredMap
{
    public sealed class MoveToRegionRequest
    {
        internal event EventHandler<MoveToRegionRequestedEventArgs> MoveToRegionRequested;

        public void MoveToRegion(MapSpan mapSpan, bool animated = true)
        {
            MoveToRegionRequested?.Invoke(this, new MoveToRegionRequestedEventArgs(mapSpan, animated));
        }
    }
}