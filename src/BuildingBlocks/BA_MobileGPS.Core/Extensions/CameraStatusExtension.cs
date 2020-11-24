﻿using BA_MobileGPS.Utilities.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.Extensions
{
    public static class CameraStatusExtension
    {
        public static bool IsLiveStreaming(int state)
        {
            return true;
        }

        public static bool IsRestreaming(int state)
        {

            if (state == (int)VideoStatusEnum.Restreaming 
                || state == (int)VideoStatusEnum.LiveStreamAndRestream 
                || state == (int)VideoStatusEnum.RestreamAndUpload
                || state == (int)VideoStatusEnum.LiveStreamAndRestreamAndUpload)
            {
                return true;
            }
            else return false;
        }
        //private int[] ConvertToBit(int state)
        //{
        //    BitArray b = new BitArray(new byte[] { state });
        //    int[] bits = b.Cast<bool>().Select(bit => bit ? 1 : 0).ToArray();
        //}
    }
}
