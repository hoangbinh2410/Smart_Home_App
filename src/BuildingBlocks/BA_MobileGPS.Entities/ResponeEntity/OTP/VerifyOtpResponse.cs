namespace BA_MobileGPS.Entities
{
    public class VerifyOtpResponse : BaseResponse<ResultVerifyOtp>
    {
        public long NewUserSecuritySMSLogID { get; set; }
    }
}