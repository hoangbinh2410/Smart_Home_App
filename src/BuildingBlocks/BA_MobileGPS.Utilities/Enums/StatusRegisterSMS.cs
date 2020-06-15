namespace BA_MobileGPS.Utilities.Enums
{
    public enum StatusRegisterSMS
    {
        OverCountOneDay = -2, // vượt quá số lần gửi trong ngày
        WasRegisterSuccess = -1, // trước đó đã kích hoạt thành công
        Success = 1, // gửi yêu cầu xác thực thành công
        ErrorSendSMS = 2, // lỗi khi gửi thông tin sms
        ErrorLogSMS = 3 // lỗi khi lưu thông tin mã xác thực
    }

    public enum StateVerifyCode
    {
        None = 0,

        /// <summary>
        /// The success
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  2/20/2019   created
        /// </Modified>
        Success = 1,

        /// <summary>
        /// nhập sai mã xác thực
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  2/20/2019   created
        /// </Modified>
        WrongVerifyCode = 2,

        /// <summary>
        /// quá thời gian nhập
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  2/20/2019   created
        /// </Modified>
        TimeOut = 3,

        /// <summary>
        /// nhập quá số lần cho phép sai của mã xác thực này
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  2/20/2019   created
        /// </Modified>
        OverWrongPerCode = 4,

        /// <summary>
        /// nhập quá số lần cho phép sai trong ngày
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// GIANGLT  2/20/2019   created
        /// </Modified>
        OverWrongPerDay = 5
    }
}