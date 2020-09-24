//using Acr.UserDialogs;

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
    public class VehicleGroupLookUpViewModel : ViewModelBase
    {
        // Lưu lại nhóm xe đã chọn
        private int[] SelectedVehicleGroups = null;

        private List<VehicleGroupModel> ListVehicleGroupOrigin = new List<VehicleGroupModel>();

        private ObservableCollection<VehicleGroupModel> listVehicleGroup = new ObservableCollection<VehicleGroupModel>();
        public ObservableCollection<VehicleGroupModel> ListVehicleGroup { get => listVehicleGroup; set => SetProperty(ref listVehicleGroup, value); }

        private bool hasVehicleGroup = true;
        public bool HasVehicleGroup { get => hasVehicleGroup; set => SetProperty(ref hasVehicleGroup, value); }

        private readonly IVehicleOnlineService vehicleOnlineService;

        private CancellationTokenSource cts;

        public ICommand SearchVehicleGroupCommand { get; private set; }

        public VehicleGroupLookUpViewModel(INavigationService navigationService, IVehicleOnlineService vehicleOnlineService)
            : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;

            SearchVehicleGroupCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicleGroup);
        }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            InitData();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null)
            {
                if (parameters.TryGetValue(ParameterKey.VehicleGroupsSelected, out int[] VehicleGroups))
                {
                    SelectedVehicleGroups = VehicleGroups;
                }
            }  
        }

        private void InitData()
        {
            if (IsBusy || !IsConnected)
                return;

            //UserDialogs.Instance.ShowLoading("");

            Task.Run(async () =>
            {
                // Lấy danh sách nhóm đội của công ty
                return await vehicleOnlineService.GetListVehicleGroupAsync(UserInfo.UserId, CurrentComanyID);
            }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                //UserDialogs.Instance.HideLoading();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    try
                    {
                        ListVehicleGroupOrigin = new List<VehicleGroupModel>();
                        ListVehicleGroup = new ObservableCollection<VehicleGroupModel>();

                        if (task.Result != null && task.Result.Count > 0)
                        {
                            // Gán vào danh sách tìm kiếm
                            ListVehicleGroupOrigin = task.Result;

                            foreach (var g in ListVehicleGroupOrigin)
                            {
                                g.NumberOfVehicle = StaticSettings.ListVehilceOnline.FindAll(v => v.GroupIDs.Split(',').Contains(g.FK_VehicleGroupID.ToString())).Count;
                            }

                            // Đánh dấu đã chọn
                            if (SelectedVehicleGroups != null)
                            {
                                ListVehicleGroupOrigin.ToList().ForEach(vg =>
                                {
                                    if (SelectedVehicleGroups.ToList().Exists(st => st == vg.FK_VehicleGroupID))
                                    {
                                        vg.IsSelected = true;
                                    }
                                    else
                                    {
                                        vg.IsSelected = false;
                                    }
                                });

                                ListVehicleGroupOrigin = ListVehicleGroupOrigin.OrderByDescending(vg => vg.IsSelected).ToList();
                            }

                            void FuckYourSelf(List<VehicleGroupModel> vehicleGroups, int level)
                            {
                                foreach (var group in vehicleGroups)
                                {
                                    for (int i = 0; i < level; i++)
                                    {
                                        group.Name = "-" + group.Name;
                                    }

                                    ListVehicleGroup.Add(group);

                                    if (level >= 10)
                                        return;

                                    FuckYourSelf(ListVehicleGroupOrigin.FindAll(g => g.ParentVehicleGroupID == group.FK_VehicleGroupID
                                        && !ListVehicleGroup.ToList().Exists(e => e.FK_VehicleGroupID == g.FK_VehicleGroupID)), level + 1);
                                }
                            }

                            FuckYourSelf(ListVehicleGroupOrigin.FindAll(g => !ListVehicleGroupOrigin.Exists(g2 => g.FK_VehicleGroupID != g2.FK_VehicleGroupID && g.ParentVehicleGroupID == g2.FK_VehicleGroupID)), 0);

                            //ListVehicleGroup = new ObservableRangeCollection<VehicleGroupModel>(ListVehicleGroupOrigin);

                            SetFocus("SearchBar");
                        }

                        HasVehicleGroup = ListVehicleGroup.Count > 0;
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                }
                else if (task.IsFaulted)
                {
                    PageDialog.DisplayAlertAsync("", task.Exception?.GetRootException().Message, MobileResource.Common_Button_OK);
                    Logger.WriteError(MethodBase.GetCurrentMethod().Name, task.Exception?.GetRootException().Message);
                }
            }));
        }

        public void SearchVehicleGroup(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    return ListVehicleGroupOrigin.ToList();
                }
                else
                {
                    return ListVehicleGroupOrigin.FindAll(vg => vg.Name != null && vg.Name.UnSignContains(args.NewTextValue));
                }
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    ListVehicleGroup = new ObservableCollection<VehicleGroupModel>(task.Result);

                    HasVehicleGroup = ListVehicleGroup.Count > 0;
                }
            }));
        }

        public ICommand SelectedTapGroupCommand
        {
            get
            {
                return new Command<Syncfusion.ListView.XForms.ItemTappedEventArgs>((args) =>
                {
                    try
                    {
                        if (args == null)
                            return;

                        var seleted = (args.ItemData as VehicleGroupModel);
                        if (seleted != null)
                        {
                            if (seleted.IsSelected)
                            {
                                seleted.IsSelected = false;
                            }
                            else
                            {
                                seleted.IsSelected = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                });
            }
        }

        public ICommand ConfirmCommand
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
                        var listGroupSelected = ListVehicleGroupOrigin.FindAll(g => g.IsSelected == true);

                        await NavigationService.GoBackAsync(useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleGroups,  listGroupSelected.Select(g => g.FK_VehicleGroupID).ToArray()}
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
                });
            }
        }
    }
}