namespace BA_MobileGPS.Utilities.Enums
{
    /// <summary>
    /// Dựa trên bit trả về, bit số :
    ///  0: Hoạt động bình thường
    ///  1: Camera ởtrạng thái sleep, sẽwake-up khi ACC ON
    ///  2: Đang thực hiện playback
    ///  4: Đang thực hiện upload video
    /// </summary>
    public enum VideoStatusEnum
    {
        Ok = 0,
        Sleep = 1,
        Playback = 2,
        Uploading = 4,
    }
}