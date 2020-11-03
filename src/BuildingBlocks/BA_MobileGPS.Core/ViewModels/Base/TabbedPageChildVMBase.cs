using Prism;
using Prism.Navigation;
using System;

namespace BA_MobileGPS.Core.ViewModels.Base
{
    public class TabbedPageChildVMBase : ViewModelBase, IActiveAware
    {
        public event EventHandler IsActiveChanged;

        public TabbedPageChildVMBase(INavigationService navigationService) : base(navigationService)
        {
            IsActiveChanged -= OnIsActiveChanged;
            IsActiveChanged += OnIsActiveChanged;
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

        public override void OnDestroy()
        {
            IsActiveChanged -= OnIsActiveChanged;
            base.OnDestroy();
        }       

        private void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

      
    }
}