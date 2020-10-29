using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.ViewModels.Base
{
    public class TabbedPageChildVMBase : ViewModelBase
    {
        public TabbedPageChildVMBase(INavigationService navigationService) : base(navigationService)
        {
            EventAggregator.GetEvent<DestroyEvent>().Subscribe(RaiseDestroyEvent);
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

        }


        protected virtual void RaiseIsActiveChanged()
        {

        }


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            IsActive = false;
           
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsActive = true;
            base.OnNavigatedTo(parameters);
        }
    }

    public class DestroyEvent : PubSubEvent
    {

    }
}
