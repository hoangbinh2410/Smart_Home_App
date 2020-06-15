namespace BA_MobileGPS.Entities
{
    public enum ResultVerifyOtp
    {
        /// <summary>
        /// Xác thực thành công
        /// </summary>
        Success = 1,

        /// <summary>
        /// Sai mã OTP
        /// </summary>
        InCorrect = 2,

        /// <summary>
        /// Quá thời gian cho 1 lần xác thực
        /// </summary>
        TimeOut = 3,

        /// <summary>
        ///  // Quá số lần cho 1 mã OTP
        /// </summary>
        WrongPerSecurityCode = 4,

        /// <summary>
        /// // Quá số lần quy định trong 1 ngày
        /// </summary>
        WrongForUserInDay = 5,

        /// <summary>
        /// Các lỗi chưa xác định
        /// </summary>
        Error = 6
    }
}