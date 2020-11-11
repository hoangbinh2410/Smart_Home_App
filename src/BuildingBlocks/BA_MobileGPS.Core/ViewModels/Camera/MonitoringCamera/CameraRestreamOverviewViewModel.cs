using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraRestreamOverviewViewModel : ViewModelBase
    {
        public CameraRestreamOverviewViewModel(INavigationService navigationService) : base(navigationService)
        {
            selectedDate = DateTime.UtcNow;
            GotoCameraRestreamCommand = new DelegateCommand(GotoCameraRestream);
            SelectDateCommand = new DelegateCommand(SelectDate);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
        }

        private void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    SelectedDate = param.Value;
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;
            }
        }

        private void SelectDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", selectedDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
            });
        }

        private void GotoCameraRestream()
        {
            var param = new NavigationParameters()
            {
               
            };
            NavigationService.NavigateAsync("NavigationPage/CameraRestream", param,true,true);
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { 
                SetProperty(ref selectedDate, value);
                RaisePropertyChanged();
            }
        }

        public ICommand GotoCameraRestreamCommand { get; }
        public ICommand SelectDateCommand { get; }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
            base.OnDestroy();
        }
    }
}
