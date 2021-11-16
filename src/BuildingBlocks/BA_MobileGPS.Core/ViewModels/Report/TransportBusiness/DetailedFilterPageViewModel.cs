using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels.Report.TransportBusiness
{
    public class DetailedFilterPageViewModel : ViewModelBase
    {
        #region Property

        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private bool _isCheckAddress = false;
        public bool ISCheckAddress 
        { 
            get => _isCheckAddress; 
            set => SetProperty(ref _isCheckAddress, value); 
        }

        private DateTime _fromDate;
        private DateTime _toDate;
        #endregion Property

        #region Contructor

        public ICommand NavigateCommand { get; }

        public DetailedFilterPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
           
            NavigateCommand = new DelegateCommand(NavigateClicked);
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        #region PrivateMethod

        public void NavigateClicked()
        {
        }

        #endregion PrivateMethod
    }
}
