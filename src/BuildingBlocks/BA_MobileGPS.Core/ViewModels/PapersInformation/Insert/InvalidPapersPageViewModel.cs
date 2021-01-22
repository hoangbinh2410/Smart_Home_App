using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class InvalidPapersPageViewModel : ViewModelBase
    {

        public ICommand SelectRegisterDateCommand { get; }
        public ICommand SelectExpireDateCommand { get; }
        public ICommand ChangePaperTypeCommand { get; }
        public InvalidPapersPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            ChangePaperTypeCommand = new DelegateCommand(ChangePaperType);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
            {
                SelectedVehiclePlates = vehicle.PrivateCode;
            }
        }

        private string selectedVehiclePlates;
        public string SelectedVehiclePlates
        {
            get { return selectedVehiclePlates; }
            set { SetProperty(ref selectedVehiclePlates, value); }
        }

        private void ChangePaperType()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("NavigationPage/SelectPaperTypePage", null, true, true);
            });
        }

    }
}
