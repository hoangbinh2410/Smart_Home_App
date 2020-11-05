using BA_MobileGPS.Core.Constant;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class RequestMoreTimePopupViewModel : ViewModelBase
    {
        public RequestMoreTimePopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            SetTimeValueCommand = new DelegateCommand<object>(SetTimeValue);
            ClosePopupCommand = new DelegateCommand(ClosePopup);
        }

        private async void ClosePopup()
        {
            await NavigationService.GoBackAsync();
        }

        private void SetTimeValue(object obj)
        {
            try
            {
                var time = Convert.ToInt32(obj);
                NavigationService.GoBackAsync(useModalNavigation: true, animated: false, parameters: new NavigationParameters
                        {
                            { ParameterKey.RequestTime,  time}
                        });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ICommand SetTimeValueCommand { get; }
        public ICommand ClosePopupCommand { get; }
    }
}