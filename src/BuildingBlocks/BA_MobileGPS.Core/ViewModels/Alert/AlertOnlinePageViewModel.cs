using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class AlertOnlinePageViewModel : ViewModelBase
    {
        private readonly IAlertService alertService;
        private readonly IVehicleOnlineService vehicleOnlineService;

        public ICommand LoadMoreItemsCommand { get; }
        public ICommand PushAlerTypeComboboxCommand { get; private set; }
        public ICommand PushVehicleComboboxCommand { get; private set; }

        public AlertOnlinePageViewModel(INavigationService navigationService, IAlertService alertService,
            IVehicleOnlineService vehicleOnlineService)
            : base(navigationService)
        {
            this.alertService = alertService;
            this.vehicleOnlineService = vehicleOnlineService;

            if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
            {
                Title = Settings.CurrentCompany.CompanyName;
            }
            else
            {
                Title = MobileResource.Alert_Label_TilePage;
            }

            selectedAlertType = new AlertTypeModel();
            selectedVehicle = new Vehicle();

            LoadMoreItemsCommand = new DelegateCommand(LoadMoreItems, CanLoadMoreItems);
            PushAlerTypeComboboxCommand = new DelegateCommand(ExecuteAlertTypeCombobox);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            EventAggregator.GetEvent<RecieveAlertEvent>().Subscribe(OnReceiveAlertSignalR);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateCombobox);

            GetAleartType();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehiclePlate)
            {
                SelectedVehicle = vehiclePlate;
            }
            else if (parameters.ContainsKey(ParameterKey.Company) && parameters.GetValue<Company>(ParameterKey.Company) is Company company)
            {
                UpdateVehicleByCompany(company);
            }
            else if ((parameters.ContainsKey("isreload") && parameters.GetValue<bool>("isreload") is bool isReload))
            {
                if (isReload)
                {
                    SearchCommand.Execute(null);
                }
            }
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<RecieveAlertEvent>().Unsubscribe(OnReceiveAlertSignalR);
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateCombobox);
        }

        #region signalR

        private void OnReceiveAlertSignalR(AlertSignalRModel alert)
        {
            if (alert == null)
            {
                return;
            }

            TryExecute(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    List<long> vehicleIDs = new List<long>();

                    if (string.IsNullOrEmpty(SelectedVehicle.VehiclePlate) || SelectedVehicle.VehicleId == 0)
                    {
                        vehicleIDs = mListVehicle.Select(v => v.VehicleId).ToList();
                    }
                    else
                    {
                        vehicleIDs = new List<long>() { SelectedVehicle.VehicleId };
                    }

                    var alertType = StaticSettings.ListAlertType?.Find(a => a.Id == alert.WarningType);
                    var alertTypeIDs = SelectedAlertType.Id == 0 ? string.Join(",", StaticSettings.ListAlertType.Select(t => t.Id)) : SelectedAlertType.Id.ToString();

                    var model = new AlertOnlineDetailModel
                    {
                        StartTime = alert.TimeStart,
                        Content = StringHelper.StripHtml(alert.WarningContent),
                        FK_AlertTypeID = alert.WarningType,
                        IsRead = false,
                        IsProcessed = false,
                        VehiclePlate = alert.VehiclePlate,
                        FK_VehicleID = alert.VehicleId,
                        IconMobile = alertType?.IconMobile,
                        AlertName = alertType?.Name
                    };

                    if (alertTypeIDs.Contains(model.FK_AlertTypeID.ToString()) && vehicleIDs.Contains(model.FK_VehicleID))
                    {
                        var list = new List<AlertOnlineDetailModel>(ListAlert);
                        list.Insert(0, model);
                        ListAlert = list.ToObservableCollection();
                        if (ListAlert.Count % PageCount == 0)
                        {
                            PageIndex += 1;
                        }
                        TotalRow += 1;
                    }
                });
            });
        }

        public void UpdateVehicleByCompany(Company company)
        {
            using (new HUDService())
            {
                Title = company.CompanyName;

                SelectedVehicle = new Vehicle();

                SearchCommand.Execute(null);
            }
        }

        #endregion signalR

        #region property

        private List<AlertOnlineDetailModel> listAlertOrigin = new List<AlertOnlineDetailModel>();

        public List<AlertOnlineDetailModel> ListAlertOrigin
        {
            get => listAlertOrigin;
            set
            {
                SetProperty(ref listAlertOrigin, value);
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<AlertOnlineDetailModel> listAlert = new ObservableCollection<AlertOnlineDetailModel>();

        public ObservableCollection<AlertOnlineDetailModel> ListAlert
        {
            get => listAlert;
            set
            {
                SetProperty(ref listAlert, value);
                RaisePropertyChanged();
            }
        }

        public int TotalRow { get; set; } = 0;

        public int PageCount { get; set; } = 20;

        public int PageIndex { get; set; } = 1;

        private Vehicle selectedVehicle;

        public Vehicle SelectedVehicle
        {
            get { return selectedVehicle; }
            set
            {
                selectedVehicle = value;
                RaisePropertyChanged();
            }
        }

        private List<VehicleOnline> mListVehicle
        {
            get
            {
                if (StaticSettings.ListVehilceOnline != null)
                {
                    return StaticSettings.ListVehilceOnline;
                }
                else
                {
                    return new List<VehicleOnline>();
                }
            }
        }

        private AlertTypeModel selectedAlertType;

        public AlertTypeModel SelectedAlertType
        {
            get { return selectedAlertType; }
            set
            {
                selectedAlertType = value;
                RaisePropertyChanged();
            }
        }

        #endregion property

        #region Command

        private bool CanLoadMoreItems()
        {
            if (ListAlert.Count <= PageIndex * PageCount || ListAlert.Count >= TotalRow || ListAlert.Count < PageCount)
                return false;
            return true;
        }

        private void LoadMoreItems()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                LoadMore();
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

        private void LoadMore()
        {
            try
            {
                var source = ListAlertOrigin.Skip(PageIndex * PageCount).Take(PageCount).ToList();
                PageIndex++;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (source != null && source.Count() > 0)
                    {
                        for (int i = 0; i < source.Count; i++)
                        {
                            ListAlert.Add(source[i]);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    SafeExecute(async () =>
                    {
                        using (new HUDService())
                        {
                            ListAlert = new ObservableCollection<AlertOnlineDetailModel>();
                            PageIndex = 0;
                            await GetListAlert();
                        }
                    });
                });
            }
        }

        public ICommand HandleAlertCommand
        {
            get
            {
                return new Command<object>((obj) =>
               {
                   SafeExecute(async () =>
                   {
                       var alertSelected = (AlertOnlineDetailModel)obj;
                       if (alertSelected != null)
                       {
                           var navigationPara = new NavigationParameters
                       {
                            { "alert", alertSelected }
                       };
                           await NavigationService.NavigateAsync("AlertHandlingPage", navigationPara);
                       }
                   });
               });
            }
        }

        public ICommand ReadAlertCommand
        {
            get
            {
                return new Command<object>((obj) =>
               {
                   SafeExecute(async () =>
                   {
                       var alertSelected = (AlertOnlineDetailModel)obj;
                       DependencyService.Get<IHUDProvider>().DisplayProgress("");
                       var isSuccess = await alertService.HandleAlertAsync(new StatusAlertRequestModel
                       {
                           PK_AlertDetailID = alertSelected.PK_AlertDetailID,
                           Status = StatusAlert.Readed,
                           ProccessContent = string.Empty,
                           UserID = UserInfo.UserId,
                           FK_AlertTypeID = alertSelected.FK_AlertTypeID,
                           FK_VehicleID = alertSelected.FK_VehicleID,
                           StartTime = alertSelected.StartTime
                       });
                       if (isSuccess)
                       {
                           GlobalResources.Current.TotalAlert--;
                           ListAlert.Remove(alertSelected);
                           DisplayMessage.ShowMessageInfo(MobileResource.Alert_Message_Alert_Success);
                       }
                       else
                       {
                           DependencyService.Get<IHUDProvider>().Dismiss();
                           DisplayMessage.ShowMessageInfo(MobileResource.Alert_Message_Alert_Fail);
                       }
                   });
               });
            }
        }

        private async Task GetListAlert()
        {
            try
            {
                string alertTypeIDs = SelectedAlertType.Id == 0 ? string.Join(",", StaticSettings.ListAlertType.Select(t => t.Id)) : SelectedAlertType.Id.ToString();

                string vehicleIDs = string.Empty;
                if (string.IsNullOrEmpty(SelectedVehicle.VehiclePlate) || SelectedVehicle.VehicleId == 0)
                {
                    vehicleIDs = string.Join(",", mListVehicle.Select(v => v.VehicleId));
                }
                else
                {
                    vehicleIDs = SelectedVehicle.VehicleId.ToString();
                }

                // Nếu không có loại cảnh báo nào thì không trả ra dữ liệu
                if (string.IsNullOrEmpty(alertTypeIDs) || string.IsNullOrEmpty(vehicleIDs))
                {
                    TotalRow = 0;
                    ListAlert = new ObservableCollection<AlertOnlineDetailModel>();
                }
                else
                {
                    var userID = UserInfo.UserId;
                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        userID = Settings.CurrentCompany.UserId;
                    }

                    var respone = await alertService.GetListAlertOnlineAsync(new AlertGetRequest
                    {
                        CompanyID = CurrentComanyID,
                        UserID = userID,
                        ListAlertTypeIDs = alertTypeIDs,
                        ListVehicleIDs = vehicleIDs,
                    });

                    if (respone != null && respone.Count > 0)
                    {
                        var lst = ListAlert.ToList();
                        respone.ForEach(a =>
                        {
                            a.Content = StringHelper.StripHtml(a.Content);
                            lst.Add(a);
                        });
                        ListAlertOrigin = lst;
                        ListAlert = lst.ToObservableCollection();
                        TotalRow = respone.Count;
                    }
                    else
                    {
                        ListAlert = new ObservableCollection<AlertOnlineDetailModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        private async void ExecuteAlertTypeCombobox()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var listAllAlertType = new List<ComboboxRequest>
                {
                    new ComboboxRequest { Key = 0, Value = MobileResource.Online_Label_StatusCarAll }
                };

                if (StaticSettings.ListAlertType != null && StaticSettings.ListAlertType.Count > 0)
                {
                    foreach (var item in StaticSettings.ListAlertType)
                    {
                        listAllAlertType.Add(new ComboboxRequest { Key = Convert.ToInt32(item.Id), Value = item.Name });
                    }
                }

                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", useModalNavigation: true, animated: true, parameters: new NavigationParameters
                {
                    { "dataCombobox", listAllAlertType },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", MobileResource.Alert_Label_AlertType }
                });
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

        private async void InitListAleart()
        {
            await GetListAlert();
        }

        private void GetAleartType()
        {
            if (StaticSettings.ListAlertType == null || StaticSettings.ListAlertType.Count == 0)
            {
                Task.Run(async () =>
                {
                    return await alertService.GetAlertTypeAsync(CurrentComanyID);
                }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        if (task.Result != null && task.Result.Count > 0)
                        {
                            StaticSettings.ListAlertType = task.Result;

                            InitListAleart();
                        }
                    }
                    else if (task.IsFaulted)
                    {
                        Logger.WriteError(task.Exception);
                    }
                }));
            }
            else
            {
                InitListAleart();
            }
        }

        public override void UpdateCombobox(ComboboxResponse param)
        {
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (short)ComboboxType.First)
                {
                    SelectedAlertType = new AlertTypeModel() { Name = param.Value, Id = param.Key };
                }
            }
        }

        #endregion Command
    }
}