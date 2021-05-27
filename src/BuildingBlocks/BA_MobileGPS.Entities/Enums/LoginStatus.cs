namespace BA_MobileGPS.Entities
{
    public enum LoginStatus
    {
        None = 0,
        Success = 1,
        LoginFailed = 2,
        UpdateRequired = 3,
        Locked = 4,
        WrongAppType = 5,
        UserLoginOnlyWeb = 6
    }
}