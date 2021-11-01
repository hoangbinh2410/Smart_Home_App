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

        private LocationStationResponse _selectedLocation = new LocationStationResponse();

        public LocationStationResponse SelectedLocation
        {
            get { return _selectedLocation; }
            set { SetProperty(ref _selectedLocation, value); }
        }

        private List<LocationStationResponse> _listLocationStation = new List<LocationStationResponse>();

        public List<LocationStationResponse> ListLocationStation
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

        public StationDetailsPageViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService
            , IStationLocationService iStationDetailsService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;
            this._iStationDetailsService = iStationDetailsService;
            PushToViewRouteCommand = new DelegateCommand<int?>(PushToViewRoute);
            PushToViewVidioCommand = new DelegateCommand<int?>(PushToViewVidio);
            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
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
            SafeExecute(async () =>
            {
                if (IsConnected)
                {
                    var companyID = CurrentComanyID;
                    using (new HUDService(MobileResource.Common_Message_Processing))
                    {
                        ListLocationStation = await _iStationDetailsService.GetListLocationStation(companyID);
                    }
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            });
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
            return new StationDetailsRequest
            {
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                LandmarkId = SelectedLocation.PK_LandmarkID,
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
            }
            return data;
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
            if (!base.CheckValidateInput(ref message))
            {
                return false;
            }
            //không chọn biển số xe
            if (string.IsNullOrEmpty(VehicleSelect.VehiclePlate))
            {
                message = MobileResource.Common_Message_NoSelectVehiclePlate;
                return false;
            }
            //không chọn địa điểm
            if (SelectedLocation == null || SelectedLocation.Name == null || SelectedLocation.PK_LandmarkID == 0)
            {
                message = MobileResource.Common_Message_PleaseSelectLocation;
                return false;
            }
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
                var model = ListDataSearch.Where(x => x.OrderNumber == obj).FirstOrDefault();
                var modelparam = new Vehicle();
                modelparam.VehiclePlate = VehicleSelect.VehiclePlate;
                modelparam.PrivateCode = VehicleSelect.PrivateCode;
                modelparam.VehicleId = VehicleSelect.VehicleId;
                var p = new NavigationParameters
                {
                    { ParameterKey.VehicleRoute, modelparam }
                };
                await NavigationService.NavigateAsync("RouteReportPage", p);
            });
        }

        /// <summary>Put data xem video.</summary>
        /// <param name="obj">The object.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  28/10/2021   created
        /// </Modified>
        private void PushToViewVidio(int? obj)
        {
            SafeExecute(async () =>
            {
                var model = ListDataSearch.Where(x => x.OrderNumber == obj).FirstOrDefault();
                var vehicleModel = new CameraLookUpVehicleModel()
                {
                    VehiclePlate = VehicleSelect.VehiclePlate,
                    VehicleId = VehicleSelect.VehicleId,
                    PrivateCode = VehicleSelect.PrivateCode,
                };
                var p = new NavigationParameters()
                        {
                            {ParameterKey.SelectDate,model.TimeInStation },
                            {ParameterKey.VehiclePlate,vehicleModel }
                        };
                var a = await NavigationService.NavigateAsync("CameraRestream", p);
            });
        }

        #endregion Menthod
    }
}