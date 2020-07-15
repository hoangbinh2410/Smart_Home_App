using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class AlertTimeSettingPageViewModel : ViewModelBase
    {
        private readonly IAlertService alertService;

        private ObservableCollection<AlertTimeConfigRespone> listTime;
        public ObservableCollection<AlertTimeConfigRespone> ListTime { get => listTime; set => SetProperty(ref listTime, value); }

        private ObservableCollection<AlertTimeConfigRespone> listTimeCheckBox;
        public ObservableCollection<AlertTimeConfigRespone> ListTimeCheckBox { get => listTimeCheckBox; set => SetProperty(ref listTimeCheckBox, value); }

        private AlertTimeConfigRespone selectedStartTime;
        public AlertTimeConfigRespone SelectedStartTime { get => selectedStartTime; set => SetProperty(ref selectedStartTime, value); }

        private AlertTimeConfigRespone selectedEndTime;
        public AlertTimeConfigRespone SelectedEndTime { get => selectedEndTime; set => SetProperty(ref selectedEndTime, value); }

        private bool isAllTimeSelected;

        public bool IsAllTimeSelected { get => isAllTimeSelected; set => SetProperty(ref isAllTimeSelected, value); }

        public ICommand TapListTimeCommand { get; private set; }

        public ICommand SelectedAllTimeCommand { get; private set; }

        public ICommand StartTimeSelectedCommand { get; private set; }

        public ICommand EndTimeSelectedCommand { get; private set; }

        public ICommand SendConfigCommand { get; private set; }

        public AlertUserConfigurationsRequest AlertConfigRequest { get; set; }

        public bool IsGetTime { get; set; } = false;

        public bool IsTickGetTime { get; set; } = false;

        public AlertTimeSettingPageViewModel(INavigationService navigationService, IAlertService alertService) : base(navigationService)
        {
            this.alertService = alertService;
            TapListTimeCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedListTime);
            SelectedAllTimeCommand = new DelegateCommand(SelectedAllTime);
            StartTimeSelectedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(StartTimeSelected);
            EndTimeSelectedCommand = new Command<Syncfusion.XForms.ComboBox.SelectionChangedEventArgs>(EndTimeSelected);
            SendConfigCommand = new DelegateCommand(SendConfig);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(ParameterKey.AlertVehicleConfig, out AlertUserConfigurationsRequest alertConfigRequest))
            {
                AlertConfigRequest = alertConfigRequest;

                GetTime();
            }
        }

        private void SendConfig()
        {
            SafeExecute(() =>
            {
                DependencyService.Get<IHUDProvider>().DisplayProgress("");

                Task.Run(async () =>
                {
                    var respones = ListTime.Where(l => l.IsVisible).ToList();

                    var ReceiveTimes = string.Empty;

                    for (int i = 0; i < respones.Count; i++)
                    {
                        if (i == (respones.Count - 1))
                        {
                            ReceiveTimes += string.Format("{0}", respones[i].TimeId);
                        }
                        else
                        {
                            ReceiveTimes += string.Format("{0},", respones[i].TimeId);
                        }
                    }

                    return await alertService.SendAlertUserConfig(new AlertUserConfigurationsRequest()
                    {
                        FK_CompanyID = AlertConfigRequest.FK_CompanyID,
                        FK_UserID = AlertConfigRequest.FK_UserID,
                        AlertTypeIDs = AlertConfigRequest.AlertTypeIDs,
                        VehicleIDs = AlertConfigRequest.VehicleIDs,
                        ReceiveTimes = ReceiveTimes
                    });
                }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IHUDProvider>().Dismiss();

                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        if (task != null)
                        {
                            if (task.Result.Data)
                            {
                                PageDialog.DisplayAlertAsync(MobileResource.AlertConfig_Label_Alter, MobileResource.AlertConfig_Label_SendSuccess,
                                      MobileResource.Common_Button_Close);

                                NavigationService.GoBackAsync(useModalNavigation: true);
                            }
                            else
                            {
                                DisplayMessage.ShowMessageError(MobileResource.AlertConfig_Label_SendFail);
                            }
                        }
                    }
                }));
            });
        }

        private void StartTimeSelected(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs obj)
        {
            GetCheckTime(IsGetTime);
        }

        private void EndTimeSelected(Syncfusion.XForms.ComboBox.SelectionChangedEventArgs obj)
        {
            GetCheckTime(IsGetTime);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsGetTime = true;
        }

        private void SelectedAllTime()
        {
            IsAllTimeSelected = !IsAllTimeSelected;
            foreach (var item in ListTime)
            {
                item.IsVisible = IsAllTimeSelected;
            }

            if (IsAllTimeSelected)
            {
                SelectedStartTime = ListTime.FirstOrDefault();

                SelectedEndTime = ListTime.LastOrDefault();
            }
            else
            {
                SelectedStartTime = ListTime.FirstOrDefault();

                SelectedEndTime = ListTime.FirstOrDefault();
            }
        }

        private void SelectedListTime(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(() =>
            {
                var selected = (args.ItemData as AlertTimeConfigRespone);
                if (selected != null)
                {
                    foreach (var item in ListTime)
                    {
                        if (item.TimeId == selected.TimeId)
                        {
                            item.IsVisible = !selected.IsVisible;
                            break;
                        }
                    }
                    var check = ListTimeCheckBox.All(l => l.IsVisible);
                    IsAllTimeSelected = check;

                    //var start = ListTimeCheckBox.Where(x => x.IsVisible).FirstOrDefault();
                    //if (start != null)
                    //{
                    //    SelectedStartTime = start;
                    //}
                    //else
                    //{
                    //    SelectedStartTime = ListTime.FirstOrDefault();
                    //}

                    //var end = ListTimeCheckBox.Where(x => x.IsVisible).LastOrDefault();
                    //if (end != null)
                    //{
                    //    SelectedEndTime = end;
                    //}
                    //else
                    //{
                    //    SelectedEndTime = ListTime.FirstOrDefault();
                    //}
                }
            });
        }

        private void GetTime()
        {
            ListTime = new ObservableCollection<AlertTimeConfigRespone>();

            for (int i = 0; i < 24; i++)
            {
                ListTime.Add(new AlertTimeConfigRespone
                {
                    TimeId = i,
                    TimeName = ConvertLengTime(i.ToString(), 0),
                    TimeNameBox = ConvertLengTime(i.ToString(), 1),
                    IsVisible = false
                });
            }

            ListTimeCheckBox = ListTime.ToObservableCollection();

            if (AlertConfigRequest.ReceiveTimes.Length > 0 && AlertConfigRequest.ReceiveTimes != null)
            {
                var split = AlertConfigRequest.ReceiveTimes.Split(',');

                foreach (var itemsplit in split)
                {
                    foreach (var item in ListTimeCheckBox)
                    {
                        if (item.TimeId == int.Parse(itemsplit))
                        {
                            item.IsVisible = true;
                            break;
                        }
                    }
                }

                SelectedStartTime = ListTimeCheckBox.Where(x => x.IsVisible).FirstOrDefault();

                SelectedEndTime = ListTimeCheckBox.Where(x => x.IsVisible).LastOrDefault();
            }
            else
            {
                IsGetTime = true;

                SelectedStartTime = ListTime.FirstOrDefault();

                SelectedEndTime = ListTime.LastOrDefault();
            }
        }

        private string ConvertLengTime(string number, int type)
        {
            if (number.Length == 1)
            {
                number = type == 0 ? "0" + number + ":00" : number + "h";
            }
            else
            {
                number = type == 0 ? number + ":00" : number + "h";
            }
            return number;
        }

        private void GetCheckTime(bool isTime)
        {
            TryExecute(async () =>
            {
                if (isTime)
                {
                    if (SelectedStartTime.TimeId > SelectedEndTime.TimeId)
                    {
                        await PageDialog.DisplayAlertAsync(MobileResource.AlertConfig_Label_Alter, MobileResource.AlertConfig_Label_Warning_Time, MobileResource.Common_Button_OK);
                    }
                    else
                    {
                        var start = ListTimeCheckBox.Where(x => x.IsVisible).FirstOrDefault();
                        var end = ListTimeCheckBox.Where(x => x.IsVisible).LastOrDefault();
                        if (start != null && end != null)
                        {
                            if (ListTimeCheckBox.Where(x => x.IsVisible).FirstOrDefault().TimeId != selectedStartTime.TimeId ||
                             ListTimeCheckBox.Where(x => x.IsVisible).LastOrDefault().TimeId != SelectedEndTime.TimeId)
                            {
                                IsTickGetTime = false;
                            }
                            else
                            {
                                IsTickGetTime = true;
                            }
                        }
                        else
                        {
                            IsTickGetTime = false;
                        }

                        foreach (var item in ListTimeCheckBox)
                        {
                            if (item.TimeId >= selectedStartTime.TimeId && item.TimeId <= SelectedEndTime.TimeId)
                            {
                                if (selectedStartTime.TimeId == 0 && SelectedEndTime.TimeId == 0)
                                {
                                    item.IsVisible = false;
                                }
                                else
                                {
                                    if (IsTickGetTime == false)
                                    {
                                        item.IsVisible = true;
                                    }
                                }
                            }
                            else
                            {
                                item.IsVisible = false;
                            }
                        }
                    }
                    var check = ListTimeCheckBox.All(l => l.IsVisible);
                    IsAllTimeSelected = check;
                }
            });
        }
    }
}