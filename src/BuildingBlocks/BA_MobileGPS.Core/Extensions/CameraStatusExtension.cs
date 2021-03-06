using BA_MobileGPS.Utilities.Enums;
using System;

namespace BA_MobileGPS.Core.Extensions
{
    public static class CameraStatusExtension
    {
        public static bool IsRestreaming(int state)
        {
            if (state == (int)VideoStatusEnum.Playback || state==(int)VideoStatusEnum.PlaybackAccOff)
            {
                return true;
            }
            else return false;
        }
    }
}