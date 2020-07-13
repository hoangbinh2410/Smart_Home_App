using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.Behaviors.ClusteredMap
{
    public sealed class AnimateCameraRequest
    {
        internal AnimateCameraBehavior AnimateCameraBehavior { get; set; }

        public Task<AnimationStatus> AnimateCamera(CameraUpdate cameraUpdate, TimeSpan? duration = null)
        {
            if (AnimateCameraBehavior == null) throw new InvalidOperationException("Not binding to AnimateCameraBehavior.");

            return AnimateCameraBehavior.AnimateCamera(cameraUpdate, duration);
        }
    }
}