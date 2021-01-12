using BA_MobileGPS.Core.Constant;
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
   
        public InvalidPapersPageViewModel(INavigationService navigationService) : base(navigationService)
        {
   
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


   


    }
}
