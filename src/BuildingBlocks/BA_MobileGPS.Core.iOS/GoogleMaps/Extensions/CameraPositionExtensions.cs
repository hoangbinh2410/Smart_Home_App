﻿using GCameraPosition = Google.Maps.CameraPosition;

namespace BA_MobileGPS.Core.iOS.Extensions
{
    public static class CameraPositionExtensions
    {
        public static CameraPosition ToXamarinForms(this GCameraPosition self)
        {
            return new CameraPosition(
                    self.Target.ToPosition(),
                    self.Zoom,
                    self.Bearing,
                    self.ViewingAngle
            );
        }

        public static GCameraPosition ToIOS(this CameraPosition self)
        {
            return new GCameraPosition(self.Target.ToCoord(), (float)self.Zoom, (float)self.Bearing, (float)self.Tilt);
        }
    }
}