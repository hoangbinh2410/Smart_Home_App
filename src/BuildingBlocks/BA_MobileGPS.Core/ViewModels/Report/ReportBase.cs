using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.XlsIO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public abstract class ReportBase<TInputModel, TService, TResult> : ViewModelBase
        where TInputModel : class, new()
        where TService : ServiceBase<TInputModel, TResult>, new()
        where TResult : class, new()
    {
        protected ReportBase(INavigationService navigationService) : base(navigationService)
        {
            Title = string.IsNullOrEmpty(Title) ? MobileResource.Common_Label_BAGPS : Title;

            InitForm();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            EventAggregator.GetEvent<SelectDateEvent>().Subscribe(UpdateDate);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Subscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateCombobox);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehiclePlate)
                {
                    VehicleSelect = vehiclePlate;
                }
                else if (parameters.ContainsKey(ParameterKey.VehicleGroups) && parameters.GetValue<int[]>(ParameterKey.VehicleGroups) is int[] vehiclegroup)
                {
                    VehicleGroups = vehiclegroup;
                    VehicleSelect = new Vehicle();
                    ListDataSearch = new ObservableCollection<TResult>();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<SelectDateEvent>().Unsubscribe(UpdateDate);
            EventAggregator.GetEvent<SelectDateTimeEvent>().Unsubscribe(UpdateDateTime);
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateCombobox);
        }

        public virtual void InitForm()
        {
            try
            {
                SearchDataCommand = new DelegateCommand(ExcuteSearchData);
                LoadMoreDataCommand = new DelegateCommand(ExcuteLoadMoreData);
                ExportExcellCommand = new DelegateCommand(ExcuteExportExcell);
                PushToFromDatePageCommand = new DelegateCommand(ExecuteToFromDate);
                PushToEndDatePageCommand = new DelegateCommand(ExecuteToEndDate);
                PushToFromDateTimePageCommand = new DelegateCommand(ExecuteToFromDateTime);
                PushToEndDateTimePageCommand = new DelegateCommand(ExecuteToEndDateTime);
                SaveComlumnHideCommand = new DelegateCommand(ExecuteSaveComlumnHide);
                SelectVehicleReportCommand = new DelegateCommand(SelectVehicleReport);
                // ẩn hiện button export excell theo quyền
                //if (CheckPermision((int)PermissionKeyNames.ReportMachineStateExport))
                //    IsEnableButtonExcell = true;
                //else
                //    IsEnableButtonExcell = false;

                vehicleSelect = new Vehicle();
                listSearchData = new ObservableCollection<TResult>();
                listSearchDataPagging = new ObservableCollection<ReportBasePaging>();

                // có load dữ liệu ngay không hay là để kích vào button
                if (isShowDataAfterLoadForm)
                    ExcuteSearchData();
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #region Property

        // có load dữ liệu sau khi load form không - mặc định là false
        private readonly bool isShowDataAfterLoadForm = false;

        public virtual int ShowHideColumnTableID { get; set; }

        public virtual IDictionary<int, bool> ShowHideColumnDictionary => new Dictionary<int, bool>();

        public virtual int AddDayMinfromDate { get; set; } = MobileSettingHelper.ViewReport;

        private string reportTitle = MobileSettingHelper.ConfigTitleDefaultReport;
        public virtual string ReportTitle { get => reportTitle; set => SetProperty(ref reportTitle, value); }

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        public virtual DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private DateTime minfromDate = DateTime.Today.AddYears(-1);
        public virtual DateTime MinfromDate { get => minfromDate; set => SetProperty(ref minfromDate, value); }

        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate { get => toDate; set => SetProperty(ref toDate, value); }

        private DateTime maxtoDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59).AddDays(1);
        public virtual DateTime MaxtoDate { get => maxtoDate; set => SetProperty(ref maxtoDate, value); }

        private Vehicle vehicleSelect;
        public Vehicle VehicleSelect { get => vehicleSelect; set => SetProperty(ref vehicleSelect, value); }

        private ObservableCollection<TResult> listSearchData;
        public ObservableCollection<TResult> ListDataSearch { get => listSearchData; set => SetProperty(ref listSearchData, value); }

        private IList<ReportBasePaging> listSearchDataPagging;
        public IList<ReportBasePaging> ListSearchDataPagging { get => listSearchDataPagging; set => SetProperty(ref listSearchDataPagging, value); }

        private bool hasData = true;
        public bool HasData { get => hasData; set => SetProperty(ref hasData, value); }

        private int pagedNext = MobileSettingHelper.ConfigPageNextReport;
        public virtual int PagedNext { get => pagedNext; set => SetProperty(ref pagedNext, value); }

        public virtual int PageSize { get; set; } = MobileSettingHelper.ConfigPageSizeReport;

        public virtual int CountMinutesShowMessageReport { get; set; } = MobileSettingHelper.ConfigCountMinutesShowMessageReport;

        private string searchKey;
        public string SearchKey { get => searchKey; set => SetProperty(ref searchKey, value); }

        private bool isJoinDay = false;
        public bool IsJoinDay { get => isJoinDay; set => SetProperty(ref isJoinDay, value); }

        // permission view
        public virtual int PermissionKeyView { get; set; }

        // permission export
        public virtual int PermissionKeyExport { get; set; }

        private bool isEnableButtonExcell = MobileSettingHelper.ConfigIsEnableButtonExport;
        public bool IsEnableButtonExcell { get => isEnableButtonExcell; set => SetProperty(ref isEnableButtonExcell, value); }

        private string tempFileName = string.Empty;
        public string TempFileName { get => tempFileName; set => SetProperty(ref tempFileName, value); }

        private int tempRowNumber = 0;
        public int TempRowNumber { get => tempRowNumber; set => SetProperty(ref tempRowNumber, value); }

        #endregion Property

        #region Command

        public ICommand PushToFromDatePageCommand { get; private set; }
        public ICommand PushToEndDatePageCommand { get; private set; }
        public ICommand PushToFromDateTimePageCommand { get; private set; }
        public ICommand PushToEndDateTimePageCommand { get; private set; }
        public ICommand SearchDataCommand { get; private set; }
        public ICommand LoadMoreDataCommand { get; private set; }
        public ICommand ExportExcellCommand { get; private set; }
        public ICommand SaveComlumnHideCommand { get; private set; }
        public DelegateCommand SelectVehicleReportCommand { get; private set; }

        #endregion Command

        #region Instance

        private TService baseServiceReport = null;

        protected TService BaseServiceReport
        {
            get
            {
                if (baseServiceReport == null)
                {
                    baseServiceReport = new TService();
                }
                return baseServiceReport;
            }
        }

        #endregion Instance

        #region Funtion

        protected virtual async void ExcuteSearchData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                string message = string.Empty;
                if (!CheckValidateInput(ref message))
                {
                    DisplayMessage.ShowMessageInfo(message, CountMinutesShowMessageReport);

                    ListDataSearch = new ObservableCollection<TResult>();
                }
                else
                {
                    using (new HUDService())
                    {
                        TempRowNumber = 0;
                        PagedNext = MobileSettingHelper.ConfigPageNextReport + 1;
                        var tempListData = await GetData();
                        // gọi hàm chuyển dữ liệu trước khi gán lên form
                        if (tempListData != null && tempListData.Count > 0)
                        {
                            tempListData = ConvertDataBeforeDisplay(tempListData);
                            ListDataSearch = new ObservableCollection<TResult>(tempListData);
                        }
                        else
                        {
                            ListDataSearch = new ObservableCollection<TResult>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorSearch, CountMinutesShowMessageReport);
            }
            finally
            {
                IsJoinDay = false;
                IsBusy = false;
                HasData = ListDataSearch.Count > 0;
            }
        }

        protected virtual async void ExcuteLoadMoreData()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                string message = string.Empty;
                if (!CheckValidateInput(ref message))
                {
                    DisplayMessage.ShowMessageInfo(message, CountMinutesShowMessageReport);
                }
                else
                {
                    PagedNext += 1;
                    var tempListData = await GetData();

                    // gọi hàm chuyển dữ liệu trước khi gán lên form
                    if (tempListData != null && tempListData.Count > 0)
                    {
                        tempListData = ConvertDataBeforeDisplay(tempListData);
                        for (int i = 0; i < tempListData.Count; i++)
                        {
                            ListDataSearch.Add(tempListData[i]);
                        }                       
                    }
                    else
                    {
                        PagedNext -= 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorSearch, CountMinutesShowMessageReport);
            }
            finally
            {
                IsBusy = false;
                HasData = ListDataSearch.Count > 0;
            }
        }

        public virtual bool CheckValidateInput(ref string message)
        {
            if (DateTime.Compare(FromDate, DateTime.MinValue) < 0 || DateTime.Compare(FromDate, DateTime.MaxValue) > 0)
            {
                message = MobileResource.Common_Message_ErrorIsNotRuleFromDate;
                return false;
            }

            if (DateTime.Compare(ToDate, DateTime.MaxValue) > 0)
            {
                message = MobileResource.Common_Message_ErrorIsNotRuleToDate;
                return false;
            }

            // FromDate not > ToDate
            if (FromDate > ToDate)
            {
                message = MobileResource.Common_Message_ErrorFromDateBiggerToDate;
                return false;
            }

            if (AddDayMinfromDate != 0)
            {
                // check thời gian có vượt quá khoảng cho phép hay không
                if ((ToDate - FromDate).TotalDays > AddDayMinfromDate)
                {
                    message = string.Format(MobileResource.Common_Message_ErrorOverDateSearch, AddDayMinfromDate);
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> ValidateDateTimeReport()
        {
            bool result = true;
            try
            {
                var validate = await BaseServiceReport.ValidateDateTimeReport(UserInfo.UserId, FromDate, ToDate);
                if (validate != null)
                {
                    switch (validate.State)
                    {
                        case StateValidateReport.None:
                            result = true;
                            break;

                        case StateValidateReport.Success:
                            result = true;
                            break;

                        case StateValidateReport.OverDateConfig:
                            result = false;
                            DisplayMessage.ShowMessageInfo(validate.Message);
                            break;

                        case StateValidateReport.DateFuture:
                            result = false;
                            DisplayMessage.ShowMessageInfo(validate.Message);
                            break;

                        case StateValidateReport.FromDateOverToDate:
                            result = false;
                            DisplayMessage.ShowMessageInfo(validate.Message);
                            break;

                        default:
                            result = true;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public virtual TInputModel SetDataInput()
        {
            return new TInputModel();
        }

        public virtual async Task<IList<TResult>> GetData()
        {
            try
            {
                var isvalid = await ValidateDateTimeReport();
                if (isvalid)
                {
                    var input = SetDataInput();
                    if (input == null) input = new TInputModel();
                    var response = await BaseServiceReport.GetData(input);
                    return response ?? new List<TResult>();
                }
                else
                {
                    return new List<TResult>();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                return new List<TResult>();
            }
        }

        public virtual IList<TResult> ConvertDataBeforeDisplay(IList<TResult> data)
        {
            return new List<TResult>();
        }

        public virtual void ExecuteSaveComlumnHide()
        {
        }

        public virtual void DisplayComlumnHide()
        {
        }

        public virtual void ExecuteToFromDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", FromDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
            });
        }

        public virtual void ExecuteToEndDate()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", ToDate },
                    { "PickerType", ComboboxType.Second }
                };
                await NavigationService.NavigateAsync("SelectDateCalendar", parameters);
            });
        }

        public virtual void ExecuteToFromDateTime()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", FromDate },
                    { "PickerType", ComboboxType.First }
                };
                await NavigationService.NavigateAsync("SelectDateTimeCalendar", parameters);
            });
        }

        public virtual void ExecuteToEndDateTime()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "DataPicker", ToDate },
                    { "PickerType", ComboboxType.Second }
                };
                await NavigationService.NavigateAsync("SelectDateTimeCalendar", parameters);
            });
        }

        public virtual void UpdateDate(PickerDateResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    FromDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Second)
                {
                    ToDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Third)
                {
                }
            }
        }

        public virtual void UpdateDateTime(PickerDateTimeResponse param)
        {
            if (param != null)
            {
                if (param.PickerType == (short)ComboboxType.First)
                {
                    FromDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Second)
                {
                    ToDate = param.Value;
                }
                else if (param.PickerType == (short)ComboboxType.Third)
                {
                }
            }
        }

        public async void ExcuteExportExcell()
        {
            try
            {
                // check quyền insert vào bộ nhớ
                if (await PermissionHelper.CheckStoragePermissions())
                {
                    string message = string.Empty;
                    if (!CheckValidateInput(ref message))
                    {
                        DisplayMessage.ShowMessageInfo(message, CountMinutesShowMessageReport);
                    }
                    else
                    {
                        using (new HUDService(MobileResource.Common_Message_Processing))
                        {
                            var data = ListDataSearch;
                            if (data != null && data.Count > 0)
                            {
                                if (TempFileName != string.Empty)
                                {
                                    if (!FillDataToExcellTemp(data))
                                    {
                                        DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorExportExcell, CountMinutesShowMessageReport);
                                    }
                                    else
                                    {
                                        DisplayMessage.ShowMessageSuccess(MobileResource.Common_Message_SuccessExportExcell, CountMinutesShowMessageReport);
                                    }
                                }
                                else
                                {
                                    if (!FillDataToExcell(data))
                                    {
                                        DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorExportExcell, CountMinutesShowMessageReport);
                                    }
                                    else
                                    {
                                        DisplayMessage.ShowMessageSuccess(MobileResource.Common_Message_SuccessExportExcell, CountMinutesShowMessageReport);
                                    }
                                }
                            }
                            else
                            {
                                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NoData, CountMinutesShowMessageReport);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
                DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_ErrorExportExcell, CountMinutesShowMessageReport);
            }
        }

        public bool FillDataToExcell(IList<TResult> data)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Set the default application version as Excel 2013.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //worksheet.ImportData(data, 2, 1, false);

                    // fill title table
                    FillDataTableExcell(data, ref worksheet);

                    // worksheet.Range["A1"].Text = "Hello World";
                    // worksheet.ImportData(data, 2, 1, true);

                    //Auto-fit the columns
                    worksheet.UsedRange.AutofitColumns();

                    workbook.Version = ExcelVersion.Excel2013;

                    //Save the workbook to stream in xlsx format.
                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    workbook.Close();
                    excelEngine.Dispose();

                    //Save the stream as a file in the device and invoke it for viewing
                    Xamarin.Forms.DependencyService.Get<ISaveAndView>().SaveAndView(ReportTitle, "application/msexcel", stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return false;
        }

        public bool FillDataToExcellTemp(IList<TResult> data)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Set the default application version as Excel 2013.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                    //"App" is the class of Portable project.
                    Assembly assembly = typeof(App).GetTypeInfo().Assembly;
                    Stream fileStream = assembly.GetManifestResourceStream("BA_MobileGPS.Core.XlsIO.Template." + TempFileName);

                    //Opens the workbook
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(fileStream);

                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //worksheet.ImportData(data, 2, 1, false);

                    // fill title table
                    FillDataTableExcell(data, ref worksheet);

                    // worksheet.Range["A1"].Text = "Hello World";
                    // worksheet.ImportData(data, 2, 1, true);

                    //Auto-fit the columns
                    worksheet.UsedRange.AutofitColumns();

                    workbook.Version = ExcelVersion.Excel2013;

                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);
                    workbook.Close();
                    excelEngine.Dispose();

                    //Save the stream as a file in the device and invoke it for viewing
                    Xamarin.Forms.DependencyService.Get<ISaveAndView>().SaveAndView(ReportTitle, "application/msexcel", stream);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            return false;
        }

        public virtual void FillDataTableExcell(IList<TResult> data, ref IWorksheet worksheet)
        {
        }

        private void SelectVehicleReport()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("BaseNavigationPage/VehicleLookUp", animated: true, useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { ParameterKey.VehicleLookUpType, VehicleLookUpType.VehicleReport },
                            {  ParameterKey.VehicleGroupsSelected, VehicleGroups},
                            {  ParameterKey.VehicleStatusSelected, ListVehicleStatus}
                        });
            });
        }

        #endregion Funtion
    }
}