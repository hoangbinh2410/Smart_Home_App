using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.RequestEntity;
using BA_MobileGPS.Service;
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
    public class TransportBusinessPageViewModel : ReportBase<ActivityDetailsRequest, ActivityDetailsService, ActivityDetailsModel>
    {

        #region Property

        // cấu hình không quá số ngày cho phép để tìm kiếm dữ liệu
        public override int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;
        public override int ShowHideColumnTableID { get; set; } = (int)TableReportEnum.Details;

        // ẩn hiện cột
        private bool _showStartTime = true;
        public bool ShowStartTime { get => _showStartTime; set => SetProperty(ref _showStartTime, value); }

        private bool _showTimeActive = true;
        public bool ShowTimeActive { get => _showTimeActive; set => SetProperty(ref _showTimeActive, value); }

        private bool _showEndTime = true;
        public bool ShowEndTime { get => _showEndTime; set => SetProperty(ref _showEndTime, value); }

        private bool _showKmGPS = true;
        public bool ShowKmGPS { get => _showKmGPS; set => SetProperty(ref _showKmGPS, value); }

        private bool _showStartAddress = true;
        public bool ShowStartAddress { get => _showStartAddress; set => SetProperty(ref _showStartAddress, value); }

        private bool _showQuotaFuelConsume = true;
        public bool ShowQuotaFuelConsume { get => _showQuotaFuelConsume; set => SetProperty(ref _showQuotaFuelConsume, value); }

        private bool _showEndAddress = true;
        public bool ShowEndAddress { get => _showEndAddress; set => SetProperty(ref _showEndAddress, value); }

        private bool _showQuotaFuel = true;
        public bool ShowQuotaFuel { get => _showQuotaFuel; set => SetProperty(ref _showQuotaFuel, value); }


        private ActivityDetailsModel selectDetailsItem;
        public ActivityDetailsModel SelectDetailsItem { get => selectDetailsItem; set => SetProperty(ref selectDetailsItem, value); }

        private ObservableCollection<ShowHideColumnResponse> listShowHideComlumn;
        public ObservableCollection<ShowHideColumnResponse> ListShowHideComlumn { get => listShowHideComlumn; set => SetProperty(ref listShowHideComlumn, value); }

        #endregion

        #region Contructor

        private readonly IShowHideColumnService showHideColumnService;
        public ICommand FilterDetailsCommand { get; private set; }
        public TransportBusinessPageViewModel(INavigationService navigationService, IShowHideColumnService showHideColumnService)
            : base(navigationService)
        {
            this.showHideColumnService = showHideColumnService;

            ListShowHideComlumn = new ObservableCollection<ShowHideColumnResponse>()
            {
                new ShowHideColumnResponse() { IDColumn = 1, Value = true},
                new ShowHideColumnResponse() { IDColumn = 2, Value = false}
            };

            FilterDetailsCommand = new DelegateCommand(ExecuteFilterDetails);
            DisplayComlumnHide();
            IsExportExcel = CheckPermision((int)PermissionKeyNames.ReportActivityDetailExport);
            ToDate = DateTime.Now;
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

        public bool IsExportExcel { get; set; }

        /// <summary>Set dữ liệu đầu vào</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
        /// </Modified>
        public override ActivityDetailsRequest SetDataInput()
        {
            return new ActivityDetailsRequest
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = VehicleSelect.VehicleId.ToString(),
                FromDate = base.FromDate,
                ToDate = base.ToDate,
                IsAddress = true,
                PageIndex = base.PagedNext,
                PageSize = base.PageSize
            };
        }

        /// <summary>Converts lại số thứ tự theo trang</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  11/13/2019   created
        /// </Modified>
        public override IList<ActivityDetailsModel> ConvertDataBeforeDisplay(IList<ActivityDetailsModel> data)
        {
            int i = (PagedNext - 1) * PageSize;
            foreach (var item in data)
            {
                item.OrderNumber = ++i;
            }
            return data;
        }

        /// <summary>Truyền sang trang lọc chi tiết nâng cao</summary>
        /// <param name="OrderNumber">The order number.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
        /// </Modified>
        private void ExecuteFilterDetails()
        {
            
        }

        /// <summary>Đổ dữ liệu vào excel</summary>
        /// <param name="data">The data.</param>
        /// <param name="worksheet">The worksheet.</param>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
        /// </Modified>
        public override void FillDataTableExcell(IList<ActivityDetailsModel> data, ref IWorksheet worksheet)
        {

        }

        /// <summary>Lưu các thông tin ẩn hiện cột</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
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

        public override IDictionary<int, bool> ShowHideColumnDictionary
        {
            get
            {
                return new Dictionary<int, bool>
                {
                    { 1, ShowStartTime },
                    { 2, ShowTimeActive },
                    { 3, ShowEndTime },
                    { 4, ShowKmGPS },
                    { 5, ShowStartAddress },
                    { 6, ShowQuotaFuelConsume },
                    { 7, ShowEndAddress },
                    { 8, ShowQuotaFuel },
                };
            }
        }

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
                            ShowStartTime = item.Value;
                            break;

                        case 2:
                            ShowTimeActive = item.Value;
                            break;

                        case 3:
                            ShowEndTime = item.Value;
                            break;

                        case 4:
                            ShowKmGPS = item.Value;
                            break;

                        case 5:
                            ShowStartAddress = item.Value;
                            break;

                        case 6:
                            ShowQuotaFuelConsume = item.Value;
                            break;

                        case 7:
                            ShowEndAddress = item.Value;
                            break;

                        case 8:
                            ShowQuotaFuel = item.Value;
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Kiểm tra đầu vào</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  15/11/2021   created
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
            return true;
        }

        #endregion
    }
}