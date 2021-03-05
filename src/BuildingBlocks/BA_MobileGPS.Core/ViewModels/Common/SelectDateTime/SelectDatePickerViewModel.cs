using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Events;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectDatePickerViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public SelectDatePickerViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
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

                var date = parameters.GetValue<DateTime>("DataPicker");
                PickerType = parameters.GetValue<short>("PickerType");

                InitForm(date);
            }
            catch (Exception)
            {
            }
        }

        private ObservableCollection<object> _startdate;

        public ObservableCollection<object> StartDate

        {
            get
            {
                return _startdate;
            }
            set => SetProperty(ref _startdate, value);
        }

        private short _pickerType;

        public short PickerType
        {
            get { return _pickerType; }
            set { _pickerType = value; }
        }

        public Command ClosePagePopupCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    IsBusy = true;
                    try
                    {
                        await _navigationService.GoBackAsync();
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
        }

        public Command IgreeSelectedDateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    IsBusy = true;
                    try
                    {
                        if (StartDate != null)
                        {
                            int month = DateTime.ParseExact(StartDate[0].ToString(), "MMMM", CultureInfo.DefaultThreadCurrentCulture).Month;
                            int.TryParse(StartDate[1].ToString(), out int day);
                            int.TryParse(StartDate[2].ToString(), out int year);

                            var input = new PickerDateTimeResponse()
                            {
                                Value = new DateTime(year, month, day),
                                PickerType = PickerType
                            };
                            _eventAggregator.GetEvent<SelectDateTimeEvent>().Publish(input);
                            var param = new NavigationParameters();
                            param.Add(ParameterKey.DateResponse, input);
                            await _navigationService.GoBackAsync(param);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
        }

        private void InitForm(DateTime date)
        {
            //Select today dates
            var todaycollection = new ObservableCollection<object>
            {
                CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Date.Month)
            };
            if (date.Date.Day < 10)
                todaycollection.Add("0" + date.Date.Day);
            else
                todaycollection.Add(date.Date.Day.ToString());
            todaycollection.Add(date.Date.Year.ToString());

            this.StartDate = todaycollection;
        }
    }
}