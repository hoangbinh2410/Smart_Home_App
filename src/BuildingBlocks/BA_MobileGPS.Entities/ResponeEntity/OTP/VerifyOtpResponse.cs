namespace BA_MobileGPS.Entities
{
    public class VerifyOtpResponse : ResponseBase<ResultVerifyOtp>
    {
        public long NewUserSecuritySMSLogID { get; set; }
    }
}