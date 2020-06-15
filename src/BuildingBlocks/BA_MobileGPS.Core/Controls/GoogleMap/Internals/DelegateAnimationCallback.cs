using System;

namespace BA_MobileGPS.Core.Internals
{
    public class DelegateAnimationCallback : IAnimationCallback
    {
        private readonly Action _onFinished;
        private readonly Action _onCanceled;

        public DelegateAnimationCallback(Action onFinished, Action onCanceled)
        {
            _onFinished = onFinished;
            _onCanceled = onCanceled;
        }

        public void OnFinished()
        {
            _onFinished?.Invoke();
        }

        public void OnCanceled()
        {
            _onCanceled?.Invoke();
        }
    }
}