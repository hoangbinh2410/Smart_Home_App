using BA_MobileGPS.Utilities.Enums;
using System;

namespace BA_MobileGPS.Core.Extensions
{
    public static class CameraStatusExtension
    {
        public static bool IsRestreaming(int state)
        {
            var status = (int)Math.Pow(2, state - 1);
            if (status == (int)VideoStatusEnum.Playback)
            {
                return true;
            }
            else return false;
        }
    }
}