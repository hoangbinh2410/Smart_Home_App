using Prism;
using Prism.Events;
using Prism.Navigation;
using System;

namespace BA_MobileGPS.Core.ViewModels.Base
{
    public class TabbedPageChildVMBase : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;

        public TabbedPageChildVMBase(INavigationService navigationService) : base(navigationService)
        {
            EventAggregator.GetEvent<DestroyEvent>().Subscribe(RaiseDestroyEvent);
            IsActiveChanged -= OnIsActiveChanged;
            IsActiveChanged += OnIsActiveChanged;
        }

        ~TabbedPageChildVMBase()
        {
            IsActiveChanged -= OnIsActiveChanged;
        }

        public virtual void OnIsActiveChanged(object sender, EventArgs e)
        {
        }

        private bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }

        private void RaiseDestroyEvent()
        {
            EventAggregator.GetEvent<DestroyEvent>().Unsubscribe(RaiseDestroyEvent);
            this.OnDestroy();
        }

        public new virtual void OnDestroy()
        {
            IsActiveChanged -= OnIsActiveChanged;
        }

        private void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }

    public class DestroyEvent : PubSubEvent
    {
    }
}