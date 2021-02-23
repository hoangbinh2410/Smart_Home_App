using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

using System;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class PopupHtmlPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        public PopupHtmlPageViewModel(INavigationService navigationService,
           IEventAggregator eventAggregator) : base(navigationService)
        {
            this._eventAggregator = eventAggregator;
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                TitlePopup = string.IsNullOrEmpty(parameters.GetValue<string>("TitlePopup")) ? MobileResource.Common_Label_BAGPS : parameters.GetValue<string>("TitlePopup");
                ContentPopup = parameters.GetValue<string>("ContentPopup");
                LblButton = string.IsNullOrEmpty(parameters.GetValue<string>("TitleButton")) ? MobileResource.RegisterConsult_Button_ClosePopup : parameters.GetValue<string>("TitleButton");
            }
            catch (Exception)
            {
            }
        }

        private string _titlePopup;

        public string TitlePopup
        {
            get { return _titlePopup; }
            set { SetProperty(ref _titlePopup, value); }
        }

        private string _contentPopup;

        public string ContentPopup
        {
            get { return _contentPopup; }
            set { SetProperty(ref _contentPopup, value); }
        }

        private string _lblButton;

        public string LblButton
        {
            get { return _lblButton; }
            set { SetProperty(ref _lblButton, value); }
        }

        public DelegateCommand CancelCommand { get; private set; }

        private async void ExecuteCancel()
        {
            try
            {
                await NavigationService.GoBackAsync();
                _eventAggregator.GetEvent<SelectCancelPopupMessage>().Publish();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }
    }
}