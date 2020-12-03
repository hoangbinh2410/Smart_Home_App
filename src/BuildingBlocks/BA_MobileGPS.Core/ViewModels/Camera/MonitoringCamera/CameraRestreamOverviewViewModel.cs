using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
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
        private readonly IStreamCameraService streamCameraService;

        public ICommand GotoCameraRestreamCommand { get; }
        public ICommand SelectDateCommand { get; }

        public CameraRestreamOverviewViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            selectedDate = DateTime.UtcNow;
            GotoCameraRestreamCommand = new DelegateCommand(GotoCameraRestream);
            SelectDateCommand = new DelegateCommand(SelectDate);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            this.streamCameraService = streamCameraService;
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
          
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

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
            SafeExecute(async () =>
           {
             var res = await  NavigationService.NavigateAsync("NavigationPage/CameraRestream", param, true, true);
           });
           
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { 
                SetProperty(ref selectedDate, value, SelectDateChange);
                RaisePropertyChanged();
            }
        }

       private async void SelectDateChange()
        {
            var req = new CameraRestreamRequest()
            {
                CustomerId = 1010,
                Date = Convert.ToDateTime("2020-11-05"),
                VehicleNames = "CAM.PNC1"
            };
            var allVideo = await streamCameraService.GetListVideoOnDevice(req);
            var uploadVideo = await streamCameraService.GetListVideoOnCloud(req);
        }

        private string chartItemsSource;
        public string ChartItemsSource
        {
            get { return chartItemsSource; }
            set { SetProperty(ref chartItemsSource, value); }
        }


        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
            base.OnDestroy();
        }
    }
}
