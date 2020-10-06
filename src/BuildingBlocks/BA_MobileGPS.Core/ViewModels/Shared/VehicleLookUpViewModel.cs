using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.Data.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleLookUpViewModel : ViewModelBase
    {
        #region Property

        private bool hasVehicle = true;
        public bool HasVehicle { get => hasVehicle; set => SetProperty(ref hasVehicle, value); }

        // Lưu lại nhóm xe đã chọn
        private int[] SelectedVehicleGroups = null;

        private static List<Vehicle> ListVehicleOrigin = new List<Vehicle>();

        private List<VehicleOnline> ListResultOnline = new List<VehicleOnline>();

        private List<Vehicle> listVehicle = new List<Vehicle>();
        public List<Vehicle> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        #endregion Property

        private readonly IVehicleOnlineService vehicleOnlineService;

        private CancellationTokenSource cts;

        private VehicleLookUpType LookUpType = VehicleLookUpType.VehicleList;

        public ICommand SearchVehicleCommand { get; private set; }

        public VehicleLookUpViewModel(INavigationService navigationService,
            IVehicleOnlineService vehicleOnlineService)
            : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;

            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters != null)
            {
                if (parameters.TryGetValue(ParameterKey.VehicleLookUpType, out VehicleLookUpType type)
                    && parameters.TryGetValue(ParameterKey.VehicleGroupsSelected, out int[] VehicleGroups)
                    && parameters.TryGetValue(ParameterKey.VehicleStatusSelected, out List<VehicleOnline> VehicleStatus))
                {
                    LookUpType = type;
                    SelectedVehicleGroups = VehicleGroups;
                    ListVehicleStatus = VehicleStatus == null ? new List<VehicleOnline>() : VehicleStatus;
                    InitData();
                }
            }
        }

        private void InitData()
        {
            Task.Run(() =>
            {
                var currentCompany = Settings.CurrentCompany;

                if (LookUpType == VehicleLookUpType.VehicleRoute)
                {
                    return GetListVehicle(SelectedVehicleGroups, true);
                }
                else
                {
                    return GetListVehicle(SelectedVehicleGroups);
                }
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    SetFocus("SearchText");
                    try
                    {
                        ListVehicleOrigin.Clear();
                        ListVehicle.Clear();

                        if (task.Result != null && task.Result.Count > 0)
                        {
                            var result = task.Result.OrderBy(x => x.PrivateCode).ToList();
                            ListVehicleOrigin = result;
                            ListVehicle = result;
                        }

                        HasVehicle = ListVehicle.Count > 0;
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

        public List<Vehicle> GetListVehicle(int[] groupids, bool isRoute = false)
        {
            List<Vehicle> result = new List<Vehicle>();
            try
            {
                if (!isRoute && ListVehicleStatus != null)
                {
                    foreach (var lst in ListVehicleStatus)
                    {
                        result.Add(AddListVehicle(lst));
                    }
                }
                else
                {
                    var listOnline = StaticSettings.ListVehilceOnline.Where(x => x.MessageId != 65 && x.MessageId != 254 && x.MessageId != 128).ToList();
                    if (groupids != null && groupids.Length > 0)
                    {
                        foreach (var item in groupids)
                        {
                            ListResultOnline = listOnline.FindAll(v => v.GroupIDs.Contains(item.ToString()));
                            foreach (var lst in ListResultOnline)
                            {
                                result.Add(AddListVehicle(lst));
                            }
                        }
                    }
                    else
                    {
                        foreach (var lst in listOnline)
                        {
                            result.Add(AddListVehicle(lst));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return result.Distinct().ToList();
        }

        private Vehicle AddListVehicle(VehicleOnline listOnline)
        {
            return (new Vehicle
            {
                VehicleId = listOnline.VehicleId,
                VehiclePlate = listOnline.VehiclePlate,
                PrivateCode = listOnline.PrivateCode,
                GroupIDs = listOnline.GroupIDs,
                Imei = listOnline.Imei
            });
        }

        private void SearchVehicle(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    return ListVehicleOrigin.ToList();
                }
                else
                {
                    return ListVehicleOrigin.FindAll(vg => vg.VehiclePlate != null && vg.VehiclePlate.UnSignContains(args.NewTextValue) || vg.PrivateCode != null && vg.PrivateCode.UnSignContains(args.NewTextValue));
                }
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var result = task.Result.OrderBy(x => x.PrivateCode).ToList();
                    ListVehicle = result;

                    HasVehicle = ListVehicle.Count > 0;
                }
            }));
        }

        public Command TapListVehicleCommand
        {
            get
            {
                return new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>(async (args) =>
                {
                    if (IsBusy)
                    {
                        return;
                    }
                    IsBusy = true;

                    try
                    {
                        var selected = (args.ItemData as Vehicle);
                        if (selected != null)
                        {
                            var navigationPara = new NavigationParameters();
                            if (LookUpType == VehicleLookUpType.VehicleRoute)
                            {
                                navigationPara.Add(ParameterKey.VehicleRoute, selected);
                            }
                            else
                            {
                                navigationPara.Add(ParameterKey.Vehicle, selected);
                            }

                            await NavigationService.GoBackAsync(navigationPara, useModalNavigation: true);
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
                });
            }
        }
    }
}