namespace BA_MobileGPS.Entities
{
    public class OtpSendDataRespone
    {
        public int SecondsLeft { get; set; }

        public long UserSecuritySMSLogID { get; set; }
    }

    public class OtpDataNavigation
    {
        public int SecondsLeft { get; set; }

        public long UserSecuritySMSLogID { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class OtpVerifyDataRespone : BaseResponse<OtpSendDataRespone>
    {
    }
}