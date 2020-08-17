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
    public class AlertVehicleSettingPageViewModel : ViewModelBase
    {
        private readonly IVehicleOnlineService vehicleOnlineService;

        public ICommand TapVehicle { get; private set; }

        public ICommand PushToSettingReceiveAlertCommand { get; private set; }

        public ICommand SearchVehicleCommand { get; private set; }

        public ICommand TapListVehicleCommand { get; private set; }

        public ICommand SelectedAllVehicleCommand { get; private set; }

        public ICommand SelectedAllVehicleNotConfiguredCommand { get; private set; }

        private VehicleLookUpType LookUpType = VehicleLookUpType.VehicleRoute;

        private CancellationTokenSource cts;

        private bool isAllVehicleSelected;

        public bool IsAllVehicleSelected { get => isAllVehicleSelected; set => SetProperty(ref isAllVehicleSelected, value); }

        private bool isAllVehicleNotConfigured;

        public bool IsAllVehicleNotConfigured { get => isAllVehicleNotConfigured; set => SetProperty(ref isAllVehicleNotConfigured, value); }

        private ObservableCollection<AlertVehicleConfigRespone> ListVehicleOrigin = new ObservableCollection<AlertVehicleConfigRespone>();

        private ObservableCollection<AlertVehicleConfigRespone> listVehicle = new ObservableCollection<AlertVehicleConfigRespone>();
        public ObservableCollection<AlertVehicleConfigRespone> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        public AlertUserConfigurationsRequest AlertConfigRequest { get; set; }

        public string SeachValue { get; set; }

        public AlertVehicleSettingPageViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService) : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
            TapVehicle = new Command(PushVehicle);
            TapListVehicleCommand = new DelegateCommand<Syncfusion.ListView.XForms.ItemTappedEventArgs>(SelectedVehicle);
            SelectedAllVehicleCommand = new DelegateCommand(SelectedAllVehicle);
            SelectedAllVehicleNotConfiguredCommand = new DelegateCommand(SelectedAllVehicleNotConfigured);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(ParameterKey.AlertCompanyConfig, out AlertUserConfigurationsRequest alertConfigRequest))
            {
                AlertConfigRequest = alertConfigRequest;

                InitData();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters?.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
            {
                VehicleGroups = vehiclegroup;

                IsAllVehicleNotConfigured = false;

                UpdateVehicleByVehicleGroup();
            }
        }

        private void InitData()
        {
            if (IsBusy || !IsConnected)
                return;

            Task.Run(async () =>
            {
                var currentCompany = Settings.CurrentCompany;

                string groupid = string.Empty;
                if (VehicleGroups != null && VehicleGroups.Length > 0)
                {
                    groupid = string.Join(",", VehicleGroups);
                }

                return await vehicleOnlineService.GetListVehicle(currentCompany?.UserId ?? UserInfo.UserId, groupid, currentCompany?.FK_CompanyID ?? CurrentComanyID, LookUpType);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        ListVehicleOrigin.Clear();
                        ListVehicle.Clear();

                        if (task.Result != null && task.Result.Count > 0)
                        {
                            var result = new List<AlertVehicleConfigRespone>();
                            task.Result.ForEach(x =>
                            {
                                result.Add(new AlertVehicleConfigRespone { VehicleId = x.VehicleId, VehiclePlate = x.VehiclePlate, PrivateCode = x.PrivateCode, Imei = x.Imei, GroupIDs = x.GroupIDs });
                            });

                            if (AlertConfigRequest.VehicleIDs.Length > 0 && AlertConfigRequest.VehicleIDs != null)
                            {
                                var split = AlertConfigRequest.VehicleIDs.Split(',');

                                foreach (var itemsplit in split)
                                {
                                    foreach (var item in result)
                                    {
                                        if (item.VehicleId == int.Parse(itemsplit))
                                        {
                                            item.IsVisible = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            ListVehicleOrigin = result.ToObservableCollection();
                            ListVehicle = result.ToObservableCollection();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
                else if (task.IsFaulted)
                {
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception?.GetRootException().Message);
                }
            }));
        }

        public void UpdateVehicleByVehicleGroup()
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);
                return GetList(ListVehicleOrigin.ToList());
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var result = task.Result.OrderBy(x => x.PrivateCode).ToObservableCollection();
                    ListVehicle = result;
                    var check = ListVehicle.All(l => l.IsVisible);
                    IsAllVehicleSelected = check;
                }
            }));
        }

        private void SearchVehicle(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);
                SeachValue = args.NewTextValue;
                return GetList(ListVehicleOrigin.ToList());
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var result = task.Result.OrderBy(x => x.PrivateCode).ToObservableCollection();
                    ListVehicle = result;
                    var check = ListVehicle.All(l => l.IsVisible);
                    IsAllVehicleSelected = check;
                }
            }));
        }

        private void SelectedAllVehicle()
        {
            IsAllVehicleSelected = !IsAllVehicleSelected;
            foreach (var item in ListVehicle)
            {
                item.IsVisible = IsAllVehicleSelected;
            }
        }

        private void SelectedAllVehicleNotConfigured()
        {
            IsAllVehicleNotConfigured = !IsAllVehicleNotConfigured;

            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);
                return GetList(ListVehicleOrigin.ToList());
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var result = task.Result.OrderBy(x => x.PrivateCode).ToObservableCollection();
                    ListVehicle = result;
                    var check = ListVehicle.All(l => l.IsVisible);
                    IsAllVehicleSelected = check;
                }
            }));
        }

        private List<AlertVehicleConfigRespone> GetList(List<AlertVehicleConfigRespone> lst)
        {
            var listVehicle = lst;
            if (VehicleGroups != null && VehicleGroups.Length > 0)
            {
                // nhóm đội
                listVehicle = lst.FindAll(v => v.GroupIDs.Split(',').ToList().Exists(g => VehicleGroups.Contains(Convert.ToInt32(g))));
            }
            if (IsAllVehicleNotConfigured && AlertConfigRequest != null && AlertConfigRequest.VehicleIDs.Length > 0)
            {
                // xe chưa được cấu hình
                var split = AlertConfigRequest.VehicleIDs.Split(',');

                int[] myInts = Array.ConvertAll(split, s => int.Parse(s));

                listVehicle = listVehicle.FindAll(v => v.VehicleId.ToString().Split(',').ToList().Exists(g => !myInts.Contains(Convert.ToInt32(g))));
            }
            if (!string.IsNullOrWhiteSpace(SeachValue))
            {
                // tìm kiếm
                listVehicle = listVehicle.FindAll(v => v.VehiclePlate != null && v.VehiclePlate.UnSignContains(SeachValue) || v.PrivateCode != null && v.PrivateCode.UnSignContains(SeachValue));
            }
            return listVehicle;
        }

        private void SelectedVehicle(Syncfusion.ListView.XForms.ItemTappedEventArgs args)
        {
            SafeExecute(() =>
            {
                var selected = (args.ItemData as AlertVehicleConfigRespone);
                if (selected != null)
                {
                    foreach (var item in ListVehicle)
                    {
                        if (item.PrivateCode == selected.PrivateCode)
                        {
                            item.IsVisible = !selected.IsVisible;
                            break;
                        }
                    }
                    var check = ListVehicle.All(l => l.IsVisible);
                    IsAllVehicleSelected = check;
                }
            });
        }

        private void PushVehicle()
        {
            TryExecute(async () =>
            {
                var respones = ListVehicleOrigin.Where(l => l.IsVisible).ToList();

                if (respones != null && respones.Count > 0)
                {
                    var VehicleIDs = string.Empty;

                    for (int i = 0; i < respones.Count; i++)
                    {
                        if (i == (respones.Count - 1))
                        {
                            VehicleIDs += string.Format("{0}", respones[i].VehicleId);
                        }
                        else
                        {
                            VehicleIDs += string.Format("{0},", respones[i].VehicleId);
                        }
                    }

                    var alertConfigRequest = new AlertUserConfigurationsRequest()
                    {
                        FK_UserID = AlertConfigRequest.FK_UserID,
                        FK_CompanyID = AlertConfigRequest.FK_CompanyID,
                        AlertTypeIDs = AlertConfigRequest.AlertTypeIDs,
                        VehicleIDs = VehicleIDs,
                        ReceiveTimes = AlertConfigRequest.ReceiveTimes ?? string.Empty,
                    };

                    RedirectConfig(alertConfigRequest);
                }
                else
                {
                    await PageDialog.DisplayAlertAsync(MobileResource.AlertConfig_Label_Alter, MobileResource.AlertConfig_Label_Warning_Vehicle, MobileResource.Common_Button_OK);
                }
            });
        }

        private void RedirectConfig(AlertUserConfigurationsRequest alertConfigRequest)
        {
            TryExecute(async () =>
            {
                await NavigationService.NavigateAsync("AlertTimeSettingPage", new NavigationParameters
                {
                    { ParameterKey.AlertVehicleConfig, alertConfigRequest }
                }, useModalNavigation: false);
            });
        }
    }
}