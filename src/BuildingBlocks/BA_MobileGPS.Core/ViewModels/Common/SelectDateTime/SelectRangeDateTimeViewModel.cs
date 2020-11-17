using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SelectRangeDateTimeViewModel : ViewModelBase
    {
        public DelegateCommand AggreeCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public DelegateCommand SelectActiveStartDateCommand { get; private set; }
        public DelegateCommand SelectActiveEndDateCommand { get; private set; }

        public DelegateCommand OnSelectionChangedCommand { get; private set; }



        public SelectRangeDateTimeViewModel(INavigationService navigationService) : base(navigationService)
        {
            AggreeCommand = new DelegateCommand(IgreeSelectedTime);
            CancelCommand = new DelegateCommand(CloseTimePage);
            SelectActiveStartDateCommand = new DelegateCommand(SelectActiveStartDate);
            SelectActiveEndDateCommand = new DelegateCommand(SelectActiveEndDate);
            OnSelectionChangedCommand = new DelegateCommand(OnSelectionDateChanged);
            InitTime();
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

        }
        private void InitTime()
        {
            isActiveStartDate = true;
            startDate = DateTime.Today.Date;
            endDate = DateTime.Now;
            selectedDate = DateTime.Now;
        }

        private DateTime startDate;
        public DateTime StartDate { get => startDate; set => SetProperty(ref startDate, value); }

        private DateTime endDate;
        public DateTime EndDate { get => endDate; set => SetProperty(ref endDate, value); }

        private DateTime selectedDate;
        public DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }

        private ObservableCollection<object> _selectedtime;

        public ObservableCollection<object> SelectedTime
        {
            get
            {
                return _selectedtime;
            }
            set => SetProperty(ref _selectedtime, value, RaiseSelectedTimeChanged);
        }

        private bool isActiveStartDate = true;
        public bool IsActiveStartDate { get => isActiveStartDate; set => SetProperty(ref isActiveStartDate, value); }

        private Color bgActiveStartDate = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
        public Color BgActiveStartDate { get => bgActiveStartDate; set => SetProperty(ref bgActiveStartDate, value); }

        private Color bgActiveEndDate = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
        public Color BgActiveEndDate { get => bgActiveEndDate; set => SetProperty(ref bgActiveEndDate, value); }

        private Color textcolorActiveStartDate = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
        public Color TextcolorActiveStartDate { get => textcolorActiveStartDate; set => SetProperty(ref textcolorActiveStartDate, value); }

        private Color textcolorActiveEndDate = (Color)Prism.PrismApplicationBase.Current.Resources["TextPrimaryColor"];
        public Color TextcolorActiveEndDate { get => textcolorActiveEndDate; set => SetProperty(ref textcolorActiveEndDate, value); }


        private void RaiseSelectedTimeChanged()
        {
            if (IsActiveStartDate)
            {
                StartDate = new DateTime(SelectedDate.Year,
                    SelectedDate.Month,
                    SelectedDate.Day, int.Parse(SelectedTime[0].ToString()),
                    int.Parse(SelectedTime[1].ToString()),
                    0);
                SelectedTime = new ObservableCollection<object> { "00", "00" };
            }
            else
            {
                EndDate = new DateTime(SelectedDate.Year,
                    SelectedDate.Month,
                    SelectedDate.Day, int.Parse(SelectedTime[0].ToString()),
                    int.Parse(SelectedTime[1].ToString()),
                    0);
                SelectedTime = new ObservableCollection<object>
                    {
                        //Current hour is selected if hour is less than 13 else it is subtracted by 12 to maintain 12hour format
                       EndDate.Hour < 10 ? ("0" + EndDate.Hour) : EndDate.Hour.ToString(),
                        //Current minute is selected
                       EndDate.Minute < 10 ? ("0" + EndDate.Minute) : EndDate.Minute.ToString()
                    };
            }
        }

        private void SelectActiveStartDate()
        {
            IsActiveStartDate = true;
            BgActiveStartDate = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
            BgActiveEndDate = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
            TextcolorActiveStartDate = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
            TextcolorActiveEndDate = (Color)Prism.PrismApplicationBase.Current.Resources["TextPrimaryColor"];


        }
        private void SelectActiveEndDate()
        {
            IsActiveStartDate = false;
            BgActiveEndDate = (Color)Prism.PrismApplicationBase.Current.Resources["PrimaryColor"];
            BgActiveStartDate = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
            TextcolorActiveStartDate = (Color)Prism.PrismApplicationBase.Current.Resources["TextPrimaryColor"];
            TextcolorActiveEndDate = (Color)Prism.PrismApplicationBase.Current.Resources["WhiteColor"];
        }
        private void OnSelectionDateChanged()
        {
            if (IsActiveStartDate)
            {
                SelectedTime = new ObservableCollection<object> { "00", "00" };
                if (SelectedDate.Date == DateTime.Today.Date)
                {
                    StartDate = DateTime.Today.Date;
                }
                else
                {
                    StartDate = SelectedDate.Date;
                }
            }
            else
            {
                if (SelectedDate.Date == DateTime.Today.Date)
                {
                    SelectedTime = new ObservableCollection<object>
                    {
                        //Current hour is selected if hour is less than 13 else it is subtracted by 12 to maintain 12hour format
                        DateTime.Now.Hour < 10 ? ("0" + DateTime.Now.Hour) : DateTime.Now.Hour.ToString(),
                        //Current minute is selected
                        DateTime.Now.Minute < 10 ? ("0" + DateTime.Now.Minute) : DateTime.Now.Minute.ToString()
                    };
                    EndDate = DateTime.Now;
                }
                else
                {
                    SelectedTime = new ObservableCollection<object> { "00", "00" };
                    EndDate = SelectedDate.Date;
                }
            }
        }

        public void CloseTimePage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync(null, useModalNavigation: true, true);
            });
        }

        private void IgreeSelectedTime()
        {
            SafeExecute(async () =>
            {
                if (StartDate >= EndDate)
                {
                    DisplayMessage.ShowMessageInfo("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc");
                }
                else
                {
                    var navigationPara = new NavigationParameters();
                    var input = new PickerRangeDateTimeResponse()
                    {
                        StartTime = StartDate,
                        EndTime = EndDate
                    };
                    navigationPara.Add(ParameterKey.SelectRangeDateTime, input);
                    await NavigationService.GoBackAsync(navigationPara, useModalNavigation: true, true);
                }

            });
        }
    }
}