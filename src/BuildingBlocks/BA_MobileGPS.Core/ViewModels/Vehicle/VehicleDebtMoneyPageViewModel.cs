using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleDebtMoneyPageViewModel : ViewModelBase
    {
        private readonly IVehicleDebtMoneyService vehicleDebtMoneyService;

        public ICommand SearchCommand { get; private set; }
        public ICommand PushToVehicleStatusCommand { get; private set; }
        public ICommand PushToVehicleStatusFreeCommand { get; private set; }
        public ICommand SearchVehicleFreeCommand { get; private set; }
        public ICommand SearchVehicleDebtCommand { get; private set; }
        public ICommand HelpCommand { get; private set; }

        public VehicleDebtMoneyPageViewModel(INavigationService navigationService,
            IVehicleDebtMoneyService vehicleDebtMoneyService) : base(navigationService)
        {
            this.vehicleDebtMoneyService = vehicleDebtMoneyService;

            Title = MobileResource.VehicleDebtMoney_Label_TilePage;
            SearchVehicleDebtCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicleDebtwithText);
            SearchVehicleFreeCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicleFreewithText);
            PushToVehicleStatusCommand = new DelegateCommand(ExecutePushToVehicleStatus);
            PushToVehicleStatusFreeCommand = new DelegateCommand(ExecutePushToVehicleFreeStatus);
            HelpCommand = new DelegateCommand(ExcuteHelp);

            ListVehicleDebtMoney = new ObservableCollection<VehicleDebtMoneyResponse>();

            ListVehicleFree = new ObservableCollection<VehicleFreeResponse>();

            StatusSelected = new ComboboxResponse()
            {
                Keys = "-1",
                Value = MobileResource.Common_Lable_All
            };
            StatusFreeSelected = new ComboboxResponse()
            {
                Keys = "0",
                Value = MobileResource.VehicleDebtMoney_Message_ID0
            };

            CompanyID = CurrentComanyID;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateCombobox);

            GetAllDataVehicleDebtMonney();

            GetAllDataVehicleFreeMonney();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters?.GetValue<Company>(ParameterKey.Company) is Company company)
            {
                CompanyID = company.FK_CompanyID;

                // gọi vào server để lấy ra dữ liệu
                if (StaticSettings.ListVehilceDebtMoney != null && StaticSettings.ListVehilceDebtMoney.Count > 0)
                {
                    InitVehicleDebt(StaticSettings.ListVehilceDebtMoney.ToList());
                }
                // gọi vào server để lấy ra dữ liệu
                if (StaticSettings.ListVehilceFree != null && StaticSettings.ListVehilceFree.Count > 0)
                {
                    InitVehicleFree(StaticSettings.ListVehilceFree.ToList());
                }
            }
            else if (parameters.TryGetValue(ParameterKey.IsLoginAnnouncement, out bool init))
            {
                isLoginAnnountment = init;
            }
        }

        private bool isLoginAnnountment { get; set; }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (isLoginAnnountment)
            {
                parameters.Add(ParameterKey.IsLoginAnnouncement, true);
            }
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateCombobox);
        }

        #region property

        private CancellationTokenSource cts;
        private int totalCountVehicle;
        public int TotalCountVehicle { get => totalCountVehicle; set => SetProperty(ref totalCountVehicle, value); }

        private int totalCountVehicleFree;
        public int TotalCountVehicleFree { get => totalCountVehicleFree; set => SetProperty(ref totalCountVehicleFree, value); }

        private int selectedIndexTab = 0;
        public int SelectedIndexTab { get => selectedIndexTab; set => SetProperty(ref selectedIndexTab, value); }

        public int? CompanyID { get; set; }

        private ComboboxResponse statusSelected;
        public ComboboxResponse StatusSelected { get => statusSelected; set => SetProperty(ref statusSelected, value); }

        private ComboboxResponse statusFreeSelected;
        public ComboboxResponse StatusFreeSelected { get => statusFreeSelected; set => SetProperty(ref statusFreeSelected, value); }

        private IList<VehicleDebtMoneyResponse> listVehicleDebtMoney;
        public IList<VehicleDebtMoneyResponse> ListVehicleDebtMoney { get => listVehicleDebtMoney; set => SetProperty(ref listVehicleDebtMoney, value); }

        private IList<VehicleFreeResponse> listVehicleFree;
        public IList<VehicleFreeResponse> ListVehicleFree { get => listVehicleFree; set => SetProperty(ref listVehicleFree, value); }

        #endregion property

        #region execute command

        private List<ComboboxRequest> LoadAllStatus()
        {
            return new List<ComboboxRequest>() {
                new ComboboxRequest() { Keys = "-1" , Value = MobileResource.Common_Lable_All },
                new ComboboxRequest() { Keys = "128" , Value = MobileResource.VehicleDebtMoney_Message_ID128 },
                new ComboboxRequest() { Keys = "3,127" , Value = MobileResource.VehicleDebtMoney_Message_ID3 },
                new ComboboxRequest() { Keys = "2" , Value = MobileResource.VehicleDebtMoney_Message_ID2 },
            };
        }

        private List<ComboboxRequest> LoadAllStatusFree()
        {
            return new List<ComboboxRequest>() {
                new ComboboxRequest() { Keys = "-1" , Value = MobileResource.Common_Lable_All },
                new ComboboxRequest() { Keys = "0,1" , Value = MobileResource.VehicleDebtMoney_Message_ID0 },
                new ComboboxRequest() { Keys = "1,129" , Value = MobileResource.VehicleDebtMoney_Message_ID1 },
                new ComboboxRequest() { Keys = "128" , Value = MobileResource.VehicleDebtMoney_Message_ID128 },
                new ComboboxRequest() { Keys = "3,127" , Value = MobileResource.VehicleDebtMoney_Message_ID3 },
                new ComboboxRequest() { Keys = "2" , Value = MobileResource.VehicleDebtMoney_Message_ID2 },
            };
        }

        private void GetAllDataVehicleDebtMonney()
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }

                // gọi vào server để lấy ra dữ liệu
                if (StaticSettings.ListVehilceDebtMoney == null || StaticSettings.ListVehilceDebtMoney.Count <= 0)
                {
                    var userID = UserInfo.UserId;
                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        userID = Settings.CurrentCompany.UserId;
                    }

                    RunOnBackground(async () =>
                    {
                        return await vehicleDebtMoneyService.LoadAllVehicleDebtMoney(userID);
                    }, (response) =>
                    {
                        if (response != null && response.Count() > 0)
                        {
                            StaticSettings.ListVehilceDebtMoney = response;
                            // hàm chạy
                            InitVehicleDebt(response);
                        }
                    });
                }
                else
                {
                    // hàm chạy
                    InitVehicleDebt(StaticSettings.ListVehilceDebtMoney);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void GetAllDataVehicleFreeMonney()
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }

                // gọi vào server để lấy ra dữ liệu
                if (StaticSettings.ListVehilceFree == null || StaticSettings.ListVehilceFree.Count <= 0)
                {
                    var userID = UserInfo.UserId;
                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        userID = Settings.CurrentCompany.UserId;
                    }
                    RunOnBackground(async () =>
                    {
                        return await vehicleDebtMoneyService.LoadAllVehicleFree(userID);
                    }, (response) =>
                    {
                        if (response != null && response.Count() > 0)
                        {
                            StaticSettings.ListVehilceFree = response;
                            // hàm chạy
                            InitVehicleFree(response);
                        }
                    });
                }
                else
                {
                    // hàm chạy
                    InitVehicleFree(StaticSettings.ListVehilceFree);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void InitVehicleDebt(List<VehicleDebtMoneyResponse> lst)
        {
            ListVehicleDebtMoney = new ObservableCollection<VehicleDebtMoneyResponse>();
            var temp = new List<VehicleDebtMoneyResponse>();
            var listMsg = StatusSelected.Keys.Split(',');
            foreach (var item in lst.Where(x => (x.FK_CompanyID == CompanyID || CompanyID == null) &&
            (StatusSelected.Keys.Equals("-1") ||
            listMsg.Contains(x.MessageIdBAP.ToString(), new MyStringifiedNumberComparer()))))
            {
                switch (item.MessageIdBAP)
                {
                    case 2:
                        item.Descriptions = MobileResource.VehicleDebtMoney_Message_ID2;
                        item.SortOrder = 3;
                        break;

                    case 3:
                        item.Descriptions = MobileResource.VehicleDebtMoney_Message_ID3;
                        item.SortOrder = 2;
                        break;

                    case 127:
                        item.Descriptions = MobileResource.VehicleDebtMoney_Message_ID3;
                        item.SortOrder = 2;
                        break;

                    case 128:
                        item.Descriptions = MobileResource.VehicleDebtMoney_Message_ID128;
                        item.SortOrder = 1;
                        break;

                    default:
                        item.Descriptions = string.Empty;
                        break;
                }

                temp.Add(item);
            }
            ListVehicleDebtMoney = temp.OrderByDescending(x => x.CountExpireDate).OrderBy(x => x.SortOrder).ToObservableCollection();
            TotalCountVehicle = ListVehicleDebtMoney.Count();
        }

        private void InitVehicleFree(List<VehicleFreeResponse> lst)
        {
            ListVehicleFree = new ObservableCollection<VehicleFreeResponse>();
            var listMsg = StatusFreeSelected.Keys.Split(',');
            var listVehicle = lst.Where(x => (x.FK_CompanyID == CompanyID || CompanyID == null) && (StatusFreeSelected.Keys.Equals("-1")
              || listMsg.Contains(x.MessageIdBAP.ToString(), new MyStringifiedNumberComparer()))).OrderByDescending(x => x.CountExpireDate);
            ListVehicleFree = listVehicle.ToObservableCollection();
            TotalCountVehicleFree = ListVehicleFree.Count();
        }

        private void SearchVehicleDebtwithText(TextChangedEventArgs obj)
        {
            SafeExecute(() =>
            {
                var keySearch = string.Empty;
                if (obj != null)
                {
                    keySearch = obj.NewTextValue.ToUpper().Trim();
                }
                if (cts != null)
                    cts.Cancel(true);

                cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    await Task.Delay(500, cts.Token);

                    if (cts.IsCancellationRequested)
                        return null;

                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        CompanyID = Settings.CurrentCompany.FK_CompanyID;
                    }

                    return StaticSettings.ListVehilceDebtMoney.Where(x => (x.VehiclePlate.ToUpper().Contains(keySearch) || string.IsNullOrEmpty(keySearch))
                    && (x.FK_CompanyID == CompanyID || CompanyID == null)
                    && (StatusSelected.Keys.Equals("-1") || StatusSelected.Keys.Split(',').Contains(x.MessageIdBAP.ToString(), new MyStringifiedNumberComparer())));
                }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion && !cts.IsCancellationRequested)
                    {
                        ListVehicleDebtMoney.Clear();

                        if (task.Result != null && task.Result.Count() > 0)
                        {
                            InitVehicleDebt(task.Result.ToList());
                        }
                    }
                    else if (task.IsFaulted)
                    {
                    }
                }));
            });
        }

        private void SearchVehicleFreewithText(TextChangedEventArgs obj)
        {
            SafeExecute(() =>
            {
                var keySearch = string.Empty;
                if (obj != null)
                {
                    keySearch = obj.NewTextValue.ToUpper().Trim();
                }

                if (cts != null)
                    cts.Cancel(true);

                cts = new CancellationTokenSource();

                Task.Run(async () =>
                {
                    await Task.Delay(500, cts.Token);

                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        CompanyID = Settings.CurrentCompany.FK_CompanyID;
                    }

                    return StaticSettings.ListVehilceFree.Where(x => (x.VehiclePlate.ToUpper().Contains(keySearch) || string.IsNullOrWhiteSpace(keySearch))
                   && (x.FK_CompanyID == CompanyID || CompanyID == null)
                   && (StatusFreeSelected.Keys.Equals("0") || StatusFreeSelected.Keys.Split(',').Contains(x.MessageIdBAP.ToString(), new MyStringifiedNumberComparer())));
                }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion && !cts.IsCancellationRequested)
                    {
                        ListVehicleFree.Clear();

                        if (task.Result != null && task.Result.Count() > 0)
                        {
                            InitVehicleFree(task.Result.ToList());
                        }
                    }
                    else if (task.IsFaulted)
                    {
                    }
                }));
            });
        }

        public override void UpdateCombobox(ComboboxResponse param)
        {
            if (param != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var dataResponse = param;
                    if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                    {
                        StatusSelected = dataResponse;

                        // gọi vào server để lấy ra dữ liệu
                        if (StaticSettings.ListVehilceDebtMoney != null && StaticSettings.ListVehilceDebtMoney.Count > 0)
                        {
                            InitVehicleDebt(StaticSettings.ListVehilceDebtMoney.ToList());
                        }
                    }
                    else if (dataResponse.ComboboxType == (Int16)ComboboxType.Second)
                    {
                        StatusFreeSelected = dataResponse;
                        // gọi vào server để lấy ra dữ liệu
                        if (StaticSettings.ListVehilceFree != null && StaticSettings.ListVehilceFree.Count > 0)
                        {
                            InitVehicleFree(StaticSettings.ListVehilceFree.ToList());
                        }
                    }
                });
            }
        }

        public void ExecutePushToVehicleStatus()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "dataCombobox", LoadAllStatus() },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", MobileResource.Camera_TitleStatus }
                };
                _ = await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", parameters, useModalNavigation: true, true);
            });
        }

        public void ExecutePushToVehicleFreeStatus()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "dataCombobox", LoadAllStatusFree() },
                    { "ComboboxType", ComboboxType.Second },
                    { "Title", MobileResource.Camera_TitleStatus }
                };
                _ = await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", parameters, useModalNavigation: true, true);
            });
        }

        private async void ExcuteHelp()
        {
            try
            {
                var parameters = new NavigationParameters
                {
                    { "TitlePopup", MobileResource.Common_Label_BAGPS },
                    { "ContentPopup", MobileResource.VehicleDebtMoneyMessage_SuccessRegister },
                    { "TitleButton", MobileResource.VehicleDebtMoney_Button_ClosePopup }
                };
                await NavigationService.NavigateAsync("PopupHtmlPage", parameters);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion execute command
    }

    public class MyStringifiedNumberComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return (Int32.Parse(x) == Int32.Parse(y));
        }

        public int GetHashCode(string obj)
        {
            return Int32.Parse(obj).ToString().GetHashCode();
        }
    }
}