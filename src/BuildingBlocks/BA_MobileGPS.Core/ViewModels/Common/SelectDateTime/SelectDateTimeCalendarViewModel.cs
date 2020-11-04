using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectDateTimeCalendarViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        private DateTime oldValue;

        public DelegateCommand AggreeCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public SelectDateTimeCalendarViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
        {
            _eventAggregator = eventAggregator;

            AggreeCommand = new DelegateCommand(IgreeSelectedTimeCommand);
            CancelCommand = new DelegateCommand(CloseTimePagePopupCommand);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters == null)
                return;

            if (parameters.TryGetValue("DataPicker", out DateTime dateTime))
            {
                oldValue = dateTime;

                InitForm(dateTime);

                SelectDate = dateTime;
            }

            if (parameters.TryGetValue("PickerType", out short type))
            {
                PickerType = type;
            }
        }

        private DateTime selectDate;
        public DateTime SelectDate { get => selectDate; set => SetProperty(ref selectDate, value); }

        private DateTime selectedDate;
        public DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }

        private ObservableCollection<object> selectedtime;
        public ObservableCollection<object> SelectedTime { get => selectedtime; set => SetProperty(ref selectedtime, value); }

        public short PickerType { get; set; }

        private void InitForm(DateTime date)
        {
            //Select today dates
            SelectedDate = date;

            //Update the current time
            SelectedTime = new ObservableCollection<object>
            {
                //Current hour is selected if hour is less than 13 else it is subtracted by 12 to maintain 12hour format
                date.Hour < 10 ? ("0" + date.Hour) : date.Hour.ToString(),
                //Current minute is selected
                date.Minute < 10 ? ("0" + date.Minute) : date.Minute.ToString()
            };
        }

        public async void CloseTimePagePopupCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var input = new PickerDateTimeResponse()
                {
                    Value = oldValue,
                    PickerType = PickerType
                };
                _eventAggregator.GetEvent<SelectDateTimeEvent>().Publish(input);
                await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void IgreeSelectedTimeCommand()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                if (SelectedTime != null)
                {
                    int hourse = int.Parse(SelectedTime[0].ToString());
                    int minutes = int.Parse(SelectedTime[1].ToString());
                    var input = new PickerDateTimeResponse()
                    {
                        Value = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, hourse, minutes, 0),
                        PickerType = PickerType
                    };
                    _eventAggregator.GetEvent<SelectDateTimeEvent>().Publish(input);
                    await NavigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}