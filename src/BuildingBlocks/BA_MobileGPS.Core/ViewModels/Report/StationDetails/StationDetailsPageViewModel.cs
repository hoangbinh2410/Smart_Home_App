using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Entities.RequestEntity.Report.Station;
using BA_MobileGPS.Entities.ResponeEntity.Report.Station;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Service.Service.Report.Station;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class StationDetailsPageViewModel : ReportBase<StationDetailsRequest, StationDetailsService, StationDetailsResponse>
    {
        #region Property

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.Details;

        private bool showTimeInStation = true;

        public bool ShowTimeInStation
        {
            get => showTimeInStation; set =>
            SetProperty(ref showTimeInStation, value);
        }

        private bool showTimeOutStation = true;

        public bool ShowTimeOutStation
        {
            get => showTimeOutStation; set =>
            SetProperty(ref showTimeOutStation, value);
        }

        private bool showNameStation = true;

        public bool ShowNameStation
        {
            get => showNameStation; set =>
            SetProperty(ref showNameStation, value);
        }

        private bool showNumberMinuteOfStation = true;

        public bool ShowNumberMinuteOfStation
        {
            get => showNumberMinuteOfStation;
            set => SetProperty(ref showNumberMinuteOfStation, value);
        }

        private StationDetailsResponse selectDetailsItem;
        public StationDetailsResponse SelectDetailsItem { get => selectDetailsItem; set => SetProperty(ref selectDetailsItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }
        public bool IsExportExcel { get; set; }

        // ducpv

        private string numberOfMinute = string.Empty;

        public string NumberOfMinute
        {
            get => numberOfMinute;
            set => SetProperty(ref numberOfMinute, value);
        }

        private ComboboxResponse _selectedLocation;
        public ComboboxResponse SelectedLocation { get => _selectedLocation; set => SetProperty(ref _selectedLocation, value); }

        private List<ComboboxRequest> _listLocationStation = new List<ComboboxRequest>();

        public List<ComboboxRequest> ListLocationStation
        {
            get { return _listLocationStation; }
            set { SetProperty(ref _listLocationStation, value); }
        }

        #endregion Property

        #region Contructor

        private readonly IShowHideColumnService showHideColumnService;
        private readonly IStationLocationService _iStationDetailsService;
        public ICommand PushToViewRouteCommand { get; private set; }
        public ICommand PushToViewVidioCommand { get; private set; }
        public ICommand PushLandmarkCommand { get; private set; }

        public StationDetailsPageViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService
            , IStationLocationService iStationDetailsService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;
            this._iStationDetailsService = iStationDetailsService;
            PushToViewRouteCommand = new DelegateCommand<int?>(PushToViewRoute);
            PushToViewVidioCommand = new DelegateCommand<int?>(PushToViewVideo);
            PushLandmarkCommand = new DelegateCommand(ExecuteLandmarkCombobox);
            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };
            ToDate = DateTime.Now;
            //Xét default key = 0 => tất cả
            _selectedLocation = new ComboboxResponse()
            {
                Key = -1,
                Value = MobileResource.ReportSignalLoss_TitleStatus_All
            };

            //Ẩn hiện cột
            DisplayComlumnHide();
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportActivityDetail,
                Type = UserBehaviorType.End
            });
            //Put dữ liệu cho combobox
            GetListLocationStation();
        }

        public override void OnDestroy()
        {
            base.Dispose();
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportActivityDetail,
                Type = UserBehaviorType.Start
            });
        }

        #endregion Lifecycle

        #region Menthod

        /// <summary>Put dữ liệu cho combobox</summary>
        /// <returns>Chọn địa điểm</returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  26/10/2021   created
        /// </Modified>
        private void GetListLocationStation()
        {
            RunOnBackground(async () =>
            {
                return await _iStationDetailsService.GetListLocationStation(CurrentComanyID);
            },
             (result) =>
             {
                 if (result != null)
                 {
                     ListLocationStation.Add(new ComboboxRequest()
                     {
                         Key = -1,
                         Value = MobileResource.ReportSignalLoss_TitleStatus_All
                     });
                     foreach (var item in result.ToList())
                     {
                         ListLocationStation.Add(new ComboboxRequest()
                         {
                             Key = item.PK_LandmarkID,
                             Value = item.Name
                         });
                     }
                 }
             });
        }

        public override void UpdateCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    SelectedLocation = dataResponse;
                }
            }
        }

        public async void ExecuteLandmarkCombobox()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                var p = new NavigationParameters
                {
                    { "dataCombobox", ListLocationStation },
                    { "ComboboxType", ComboboxType.First },
                    { "Title", "Chọn điểm" }
                };
                await NavigationService.NavigateAsync("BaseNavigationPage/ComboboxPage", p, useModalNavigation: true, true);
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  26/10/2021   created
        /// </Modified>
        public override StationDetailsRequest SetDataInput()
        {
            int.TryParse(NumberOfMinute, out int numberOfMinute);
            string vehicleIDs = "";
            // không chọn xe thì lấy tất cả VehicleId
            if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                var listOnline = StaticSettings.ListVehilceOnline;
                List<long> vehicleId = new List<long>();
                if (listOnline.Count > 0)
                {
                    foreach (var item in listOnline)
                    {
                        vehicleId.Add(item.VehicleId);
                    }
                }
                vehicleIDs = string.Join(",", vehicleId);
            }
            else
            {
                vehicleIDs = VehicleSelect.VehicleId.ToString();
            }
            return new StationDetailsRequest
            {
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                CompanyID = CurrentComanyID,
                VehicleIDs = vehicleIDs,
                LandmarkId = SelectedLocation.Key,
                NumberOfMinute = numberOfMinute,
                PageSize = base.PageSize,
                PageIndex = base.PagedNext
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// Chưa xử dụng
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  26/10/2021   created
        /// </Modified>
        public override IList<StationDetailsResponse> ConvertDataBeforeDisplay(IList<StationDetailsResponse> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
                item.IsVideoCam = ValidateVehicleCamera(item.VehiclePlate);
            }
            return data;
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

        /// <summary>Đổ dữ liệu vào excel</summary>
        /// Chưa làm
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  26/10/2021   created
        /// </Modified>
        public override void FillDataTableExcell(IList<StationDetailsResponse> data, ref IWorksheet worksheet)
        {
            try
            {
                DisplayMessage.ShowMessageInfo("Đang phát triển", 5000);
                return;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        public override IDictionary<int, bool> ShowHideColumnDictionary
        {
            get
            {
                return new Dictionary<int, bool>
                {
                    { 1, ShowTimeInStation },
                    { 2, ShowTimeOutStation },
                    { 3, ShowNameStation },
                    { 4, ShowNumberMinuteOfStation },
                };
            }
        }

        /// <summary>Lưu các thông tin ẩn hiện cột</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  27/10/2021   created
        /// </Modified>
        public override void ExecuteSaveComlumnHide()
        {
            foreach (var item in ShowHideColumnDictionary)
            {
                // đẩy xuống db
                var model = showHideColumnService.Get(r => r.IDTable == ShowHideColumnTableID && r.IDColumn == item.Key);
                if (model != null)
                {
                    if (model.Value != item.Value)
                    {
                        model.Value = item.Value;
                        showHideColumnService.Update(model);
                    }
                }
                else
                {
                    // Thêm resouce vào realm db
                    showHideColumnService.Add(new ShowHideColumnResponse()
                    {
                        IDTable = ShowHideColumnTableID,
                        IDColumn = item.Key,
                        Value = item.Value
                    });
                }
            }
        }

        /// <summary>Xét ẩn hiện cột</summary>
        /// <summary>Displays the comlumn hide.
        /// Name     Date         Comments
        /// ducpv  26/10/2021   created
        /// </summary>
        public override void DisplayComlumnHide()
        {
            try
            {
                var temp = showHideColumnService.Find(r => r.IDTable == ShowHideColumnTableID);
                foreach (var item in temp)
                {
                    switch (item.IDColumn)
                    {
                        case 1:
                            ShowTimeInStation = item.Value;
                            break;

                        case 2:
                            ShowTimeOutStation = item.Value;
                            break;

                        case 3:
                            ShowNameStation = item.Value;
                            break;

                        case 4:
                            ShowNumberMinuteOfStation = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Kiểm tra chọn biển số xe</summary>
        /// <summary>Kiểm tra chọn địa điểm</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  26/10/2021   created
        /// </Modified>
        public override bool CheckValidateInput(ref string message)
        {
            //if (!base.CheckValidateInput(ref message))
            //{
            //    return false;
            //}
            ////không chọn biển số xe
            //if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            //{
            //    message = MobileResource.Common_Message_NoSelectVehiclePlate;
            //    return false;
            //}
            ////không chọn địa điểm
            //if (SelectedLocation == null || SelectedLocation.Key == 0)
            //{
            //    message = MobileResource.Common_Message_PleaseSelectLocation;
            //    return false;
            //}
            return true;
        }

        /// <summary>Put data xem lộ trình.</summary>
        /// <param name="obj">The object.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  28/10/2021   created
        /// </Modified>
        private void PushToViewRoute(int? obj)
        {
            SafeExecute(async () =>
            {
                if (CheckPermision((int)PermissionKeyNames.ViewModuleRoute))
                {
                    var model = ListDataSearch.Where(x => x.OrderNumber == obj).FirstOrDefault();
                    var modelparam = new Vehicle();
                    modelparam.VehiclePlate = VehicleSelect.VehiclePlate;
                    modelparam.PrivateCode = VehicleSelect.PrivateCode;
                    modelparam.VehicleId = VehicleSelect.VehicleId;
                    var p = new NavigationParameters
                    {
                        {"ReportDate", new Tuple<DateTime,DateTime>(model.TimeInStation,model.TimeOutStation) },
                        { ParameterKey.VehicleRoute, modelparam }
                    };
                    await NavigationService.NavigateAsync("RouteReportPage", p);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Bạn không có quyền truy cập chức năng này");
                }
            });
        }

        /// <summary>Put data xem video.</summary>
        /// <param name="obj">The object.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  28/10/2021   created
        /// </Modified>
        private void PushToViewVideo(int? obj)
        {
            SafeExecute(async () =>
            {
                if (CheckPermision(1354) || CheckPermision(1355))
                {
                    var model = ListDataSearch.Where(x => x.OrderNumber == obj).FirstOrDefault();
                    var vehicleModel = new CameraLookUpVehicleModel()
                    {
                        VehiclePlate = model.VehiclePlate,
                        VehicleId = model.FK_VehicleID,
                    };
                    var p = new NavigationParameters()
                        {
                            {"ReportDate", new Tuple<DateTime,DateTime>(model.TimeInStation,model.TimeOutStation) },
                            {ParameterKey.VehiclePlate,vehicleModel }
                        };
                    await NavigationService.NavigateAsync("NavigationPage/CameraRestream", p, true, true);
                }
                else
                {
                    DisplayMessage.ShowMessageInfo("Bạn không có quyền truy cập chức năng này");
                }
            });
        }

        #endregion Menthod
    }
}