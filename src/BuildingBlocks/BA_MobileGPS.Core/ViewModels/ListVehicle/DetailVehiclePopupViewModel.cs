using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class DetailVehiclePopupViewModel : ViewModelBase
    {
        public DetailVehiclePopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            CloseCommand = new DelegateCommand(Close);
            NavigativeCommand = new DelegateCommand<object>(Navigative);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("vehicleItem", out string obj))
                {
                    VehicleName = obj;
                }
            }
        }

        private string vehicleName;

        public string VehicleName
        {
            get { return vehicleName; }
            set { SetProperty(ref vehicleName, value); }
        }

        public ICommand CloseCommand { get; }

        private void Close()
        {
            NavigationService.GoBackAsync();
        }

        public ICommand NavigativeCommand { get; }

        private void Navigative(object obj)
        {
            if (!(obj is MenuItem seletedMenu))
            {
                return;
            }
            SafeExecute(async () =>
            {
                var param = seletedMenu.Title.ToString();
                await NavigationService.GoBackAsync(useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { "pagetoNavigation",  param}
                        });
            });
        }
    }
}