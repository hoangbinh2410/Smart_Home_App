namespace BA_MobileGPS.Core.Internals
{
    public interface IAnimationCallback
    {
        void OnFinished();

        void OnCanceled();
    }
}