using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Utilities.Enums
{
    /// <summary>
    /// Dựa trên bit trả về, bit số : 
    ///  0 : Livestream 
    ///  1 : Playback
    ///  2 : Upload video
    ///  3 : Chưa có dữ liệu từ PNC
    /// </summary>
    public enum VideoStatusEnum
    {
        LiveStreaming = 1,
        Restreaming = 2,
        LiveStreamAndRestream = 3,
        Uploading = 4,
        LiveStreamAndUpload = 5,
        RestreamAndUpload = 6,
        LiveStreamAndRestreamAndUpload = 7

    }
}
