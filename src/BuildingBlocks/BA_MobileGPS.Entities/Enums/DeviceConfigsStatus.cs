namespace BA_MobileGPS.Entities
{
    public enum DeviceConfigsStatus : byte
    {
        Success = 1,
        ChangePasswoldFalse = 2,
        UnlockFalse = 3,
        Exception = 4,
        WaitingLastCommand = 5,
        ConnectedDevice = 6
    }
}