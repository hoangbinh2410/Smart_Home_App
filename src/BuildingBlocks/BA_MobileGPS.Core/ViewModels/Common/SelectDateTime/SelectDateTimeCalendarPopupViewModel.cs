using BA_MobileGPS.Entities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectDateTimeCalendarPopupViewModel : ViewModelBase
    {
        private DateTime oldValue;

        private DateTime selectDate;
        public DateTime SelectDate { get => selectDate; set => SetProperty(ref selectDate, value); }

        private DateTime selectedDate;
        public DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }

        private ObservableCollection<object> selectedtime;
        public ObservableCollection<object> SelectedTime
        {
            get => selectedtime;
            set
            {
                SetProperty(ref selectedtime, value);
            }
        }

        public ICommand AgreeCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public SelectDateTimeCalendarPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            AgreeCommand = new DelegateCommand(AgreeSelected);
            CancelCommand = new DelegateCommand(ClosePage);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters != null && parameters.TryGetValue("DataPicker", out DateTime dateTime))
            {
                oldValue = dateTime;

                InitForm(oldValue);

                SelectDate = oldValue;
            }
        }

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

        public void ClosePage()
        {
            SafeExecute(async () =>
            {
                _ = await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
            });
        }

        private void AgreeSelected()
        {
            SafeExecute(async () =>
            {
                if (SelectedTime != null)
                {
                    _ = await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
                    EventAggregator.GetEvent<SelectDateTimeEvent>().Publish(new PickerDateTimeResponse()
                    {
                        Value = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, int.Parse(SelectedTime[0].ToString()), int.Parse(SelectedTime[1].ToString()), 0)
                    });

                }
            });
        }
    }
}