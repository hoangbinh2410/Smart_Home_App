using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Events;
using Prism.Navigation;

using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    /// <summary>
    /// Form chọn thời gian
    /// </summary>
    /// <Modified>
    /// Name     Date         Comments
    /// hoangdt  3/26/2019   created
    /// </Modified>
    public class SelectMonthCalendarViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public SelectMonthCalendarViewModel(INavigationService navigationService, IEventAggregator eventAggregator) : base(navigationService)
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

        public short PickerType { get; set; }

        private ObservableCollection<object> _selectedtime;

        public ObservableCollection<object> SelectedTime
        {
            get
            {
                return _selectedtime;
            }
            set => SetProperty(ref _selectedtime, value);
        }

        public Command CloseTimePagePopupCommand
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

        public Command IgreeSelectedTimeCommand
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
                        if (SelectedTime != null)
                        {
                            var input = new PickerDateTimeResponse()
                            {
                                Time = SelectedTime[0].ToString() + ":" + SelectedTime[1].ToString(),
                                PickerType = PickerType
                            };
                            _eventAggregator.GetEvent<SelectTimeEvent>().Publish(input);
                            await _navigationService.GoBackAsync();
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
                //Current hour is selected if hour is less than 13 else it is subtracted by 12 to maintain 12hour format
                date.Month.ToString(),

                //Current minute is selected
                date.Year.ToString()
            };

            //Update the current time
            this.SelectedTime = todaycollection;
        }
    }
}