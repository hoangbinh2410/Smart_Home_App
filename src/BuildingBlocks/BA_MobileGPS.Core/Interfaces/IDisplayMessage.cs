namespace BA_MobileGPS.Core
{
    public interface IDisplayMessage
    {
        void ShowMessageInfo(string message, double time = 5000);

        void ShowMessageWarning(string message, double time = 5000);

        void ShowMessageError(string message, double time = 5000);

        void ShowMessageSuccess(string message, double time = 5000);
    }
}