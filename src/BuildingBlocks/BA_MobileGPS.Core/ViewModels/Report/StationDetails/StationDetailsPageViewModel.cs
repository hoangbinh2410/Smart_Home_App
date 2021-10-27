﻿using BA_MobileGPS.Core.Constant;
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

        private bool showVehicleType = false;
        public bool ShowVehicleType { get => showVehicleType; set => SetProperty(ref showVehicleType, value); }

        private bool showVehiclePlate = false;
        public bool ShowVehiclePlate { get => showVehiclePlate; set => SetProperty(ref showVehiclePlate, value); }

        private bool showTripCompensatory = true;
        public bool ShowTripCompensatory { get => showTripCompensatory; set => SetProperty(ref showTripCompensatory, value); }

        private bool showCurrentTime = true;
        public bool ShowCurrentTime { get => showCurrentTime; set => SetProperty(ref showCurrentTime, value); }

        private bool showTimeActive = true;
        public bool ShowTimeActive { get => showTimeActive; set => SetProperty(ref showTimeActive, value); }

        private bool showKmGPS = true;
        public bool ShowKmGPS { get => showKmGPS; set => SetProperty(ref showKmGPS, value); }

        private bool showQuotaFuel = true;
        public bool ShowQuotaFuel { get => showQuotaFuel; set => SetProperty(ref showQuotaFuel, value); }

        private bool showKmCO = true;
        public bool ShowKmCO { get => showKmCO; set => SetProperty(ref showKmCO, value); }

        private bool showQuotaFuelConsume = true;
        public bool ShowQuotaFuelConsume { get => showQuotaFuelConsume; set => SetProperty(ref showQuotaFuelConsume, value); }

        private bool showStartAddress = true;
        public bool ShowStartAddress { get => showStartAddress; set => SetProperty(ref showStartAddress, value); }

        private bool showEndAddress = true;
        public bool ShowEndAddress { get => showEndAddress; set => SetProperty(ref showEndAddress, value); }

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

        #endregion

        #region Contructor
        private readonly IShowHideColumnService showHideColumnService;
        private readonly IStationLocationService _iStationDetailsService;
        public ICommand DetailVehicleCommand { get; private set; }
        public StationDetailsPageViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService
            , IStationLocationService iStationDetailsService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;
            this._iStationDetailsService = iStationDetailsService;
            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };
            //FromDate = DateTime.Parse("2021-10-20T00:00:00");
            //ToDate = DateTime.Parse("2021-10-25T00:00:00");
        }
        #endregion

        #region Lifecycle
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            EventAggregator.GetEvent<UserBehaviorEvent>().Publish(new UserBehaviorModel()
            {
                Page = Entities.Enums.MenuKeyEnums.ReportActivityDetail,
                Type = UserBehaviorType.End
            });
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
        #endregion


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
            int.TryParse(NumberOfMinute , out int numberOfMinute);
            return new StationDetailsRequest
            {
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                CompanyID = CurrentComanyID,
                VehicleIDs =VehicleSelect.VehicleId.ToString(), 
                LandmarkId = SelectedLocation.PK_LandmarkID,
                NumberOfMinute = numberOfMinute,
                PageSize = base.PageSize,
                PageIndex = base.PagedNext
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
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
                            ShowVehicleType = item.Value;
                            break;

                        case 2:
                            ShowVehiclePlate = item.Value;
                            break;

                        case 3:
                            ShowTripCompensatory = item.Value;
                            break;

                        case 4:
                            ShowCurrentTime = item.Value;
                            break;

                        case 5:
                            ShowTimeActive = item.Value;
                            break;

                        case 6:
                            ShowKmGPS = item.Value;
                            break;

                        case 7:
                            ShowQuotaFuel = item.Value;
                            break;

                        case 8:
                            ShowKmCO = item.Value;
                            break;

                        case 9:
                            ShowQuotaFuelConsume = item.Value;
                            break;

                        case 10:
                            ShowStartAddress = item.Value;
                            break;

                        case 11:
                            ShowEndAddress = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>Kiểm tra chọn biển số xe</summary>
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
            return true;
        }
        #endregion
    }
}