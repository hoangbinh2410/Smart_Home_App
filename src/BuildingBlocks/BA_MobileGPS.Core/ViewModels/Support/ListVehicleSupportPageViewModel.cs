using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Extensions;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListVehicleSupportPageViewModel : ViewModelBase
    {
        #region Property

        private bool _hasVehicleGroup = false;

        public bool HasVehicleGroup
        {
            get { return _hasVehicleGroup; }
            set { SetProperty(ref _hasVehicleGroup, value); }
        }

        private bool _hasChooseVehicleGroup = false;

        public bool HasChooseVehicleGroup
        {
            get { return _hasChooseVehicleGroup; }
            set { SetProperty(ref _hasChooseVehicleGroup, value); }
        }

        private List<VehicleGroupModel> _listVehicleGroup = new List<VehicleGroupModel>();

        public List<VehicleGroupModel> ListVehicleGroup
        {
            get { return _listVehicleGroup; }
            set { SetProperty(ref _listVehicleGroup, value); }
        }

        private List<VehicleOnline> ListChooseVehicleGroup = new List<VehicleOnline>();
        private SupportCategoryRespone _category;

        #endregion Property

        #region Contructor

        public ICommand NavigateCommand { get; }

        public ListVehicleSupportPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigateCommand = new DelegateCommand(NavigateClicked);
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone objCategory)
            {
                _category = objCategory;
            }
            if (_category == null)
            {
                return;
            }
            var listVehilceOnline = StaticSettings.ListVehilceOnline;
            if (_category.IsChangePlate && _category.Code == MenuSupportEnum.ChangePlatePage.ToDescription())
            {
                GetlistCarChangePlate(listVehilceOnline);
            }
            else if (_category.Code == MenuSupportEnum.ErrorSinglePage.ToDescription())
            {
                GetlistCarErrorSingle(listVehilceOnline);
            }
            else if (_category.Code == MenuSupportEnum.ErrorCameraPage.ToDescription())
            {
                GetlistCarErrorCamera(listVehilceOnline);
            }
            HasVehicleGroup = ListVehicleGroup.Count == 0;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        #region PrivateMenthod

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
                        if (_category.IsChangePlate)
                        {
                            ListVehicleGroup.Where(x => x.Name != seleted.Name)?.ToList()
                            .Select(y => { y.IsSelected = false; return y; })?.ToList();
                        }

                        if (seleted.IsSelected)
                        {
                            ListVehicleGroup.Where(x => x.Name == seleted.Name)?.ToList()
                            .Select(y => { y.IsSelected = false; return y; })?.ToList();
                        }
                        else
                        {
                            ListVehicleGroup.Where(x => x.Name == seleted.Name)?.ToList()
                            .Select(y => { y.IsSelected = true; return y; })?.ToList();
                        }

                        ListChooseVehicleGroup = new List<VehicleOnline>();
                        foreach (var item in ListVehicleGroup)
                        {
                            if (item.IsSelected)
                            {
                                var obj = StaticSettings.ListVehilceOnline.Where(x => x.VehiclePlate == item.Name).FirstOrDefault();
                                ListChooseVehicleGroup.Add(obj);
                            }
                        }
                        HasChooseVehicleGroup = ListChooseVehicleGroup.Count > 0;
                    }
                    catch (System.Exception ex)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                    }
                });
            }
        }

        private void NavigateClicked()
        {
            if (_category == null || ListChooseVehicleGroup.Count == 0)
            {
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoData, 5000);
                return;
            }

            var parameters = new NavigationParameters
            {
                { "Support", _category },
                { "ListVehicleSupport", ListChooseVehicleGroup }
            };

            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("SelectSupportPage", parameters);
            });
        }

        private void GetlistCarErrorSingle(List<VehicleOnline> listVehilceOnline)
        {
            if (listVehilceOnline != null)
            {
                foreach (var item in listVehilceOnline)
                {
                    if (StateVehicleExtension.IsLostGPS(item.GPSTime, item.VehicleTime) == true || StateVehicleExtension.IsLostGSM(item.VehicleTime) == true)
                    {
                        VehicleGroupModel obj = new VehicleGroupModel();
                        obj.Name = item.VehiclePlate;
                        ListVehicleGroup.Add(obj);
                    }
                }
            }
        }
        private void GetlistCarChangePlate(List<VehicleOnline> listVehilceOnline)
        {
            if (listVehilceOnline != null)
            {
                foreach (var item in listVehilceOnline)
                {
                    VehicleGroupModel obj = new VehicleGroupModel();
                    obj.Name = item.VehiclePlate;
                    ListVehicleGroup.Add(obj);
                }
            }
        }
        private void GetlistCarErrorCamera(List<VehicleOnline> listVehilceOnline)
        {
            if (listVehilceOnline != null)
            {
                foreach (var item in listVehilceOnline)
                {
                    if (ValidateVehicleCamera(item.VehiclePlate))
                    {
                        VehicleGroupModel obj = new VehicleGroupModel();
                        obj.Name = item.VehiclePlate;
                        ListVehicleGroup.Add(obj);
                    }
                }
            }
        }
        private bool ValidateVehicleCamera(string vehiclePlate)
        {
            var listVehicleCamera = StaticSettings.ListVehilceCamera;
            if (listVehicleCamera != null)
            {
                var plate = vehiclePlate.Contains("_C") ? vehiclePlate : vehiclePlate + "_C";
                var model = StaticSettings.ListVehilceCamera.FirstOrDefault(x => x.VehiclePlate == plate);
                if (model != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion PrivateMenthod
    }
}