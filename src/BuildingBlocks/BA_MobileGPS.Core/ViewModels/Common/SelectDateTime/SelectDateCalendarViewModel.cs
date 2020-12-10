using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

using System;
using System.Reflection;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectDateCalendarViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        private DateTime oldValue;

        public DelegateCommand AggreeCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public SelectDateCalendarViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
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

        public short PickerType { get; set; }

        private void InitForm(DateTime date)
        {
            //Select today dates
            SelectedDate = date;
        }

        public void CloseTimePagePopupCommand()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
            });
        }

        private void IgreeSelectedTimeCommand()
        {
            SafeExecute(async () =>
            {
                var input = new PickerDateResponse()
                {
                    Value = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day),
                    PickerType = PickerType
                };
                _eventAggregator.GetEvent<SelectDateEvent>().Publish(input);
                await NavigationService.GoBackAsync();

            });
        }
    }
}