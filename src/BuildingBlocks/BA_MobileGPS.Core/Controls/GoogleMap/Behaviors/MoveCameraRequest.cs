using System;
using System.Threading.Tasks;

namespace BA_MobileGPS.Core.GoogleMap.Behaviors
{
    public sealed class MoveCameraRequest
    {
        internal MoveCameraBehavior MoveCameraBehavior { get; set; }

        public Task<AnimationStatus> MoveCamera(CameraUpdate cameraUpdate)
        {
            if (MoveCameraBehavior == null) throw new InvalidOperationException("Not binding to MoveCameraBehavior.");

            return MoveCameraBehavior.MoveCamera(cameraUpdate);
        }
    }
}