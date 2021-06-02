using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public abstract class ReportBaseViewModel<TService, TRequest, TResponse> : ViewModelBase
        where TService : IReportBaseService<TRequest, TResponse>
        where TRequest : class, new()
        where TResponse : class, new()
    {
        protected readonly IReportBaseService<TRequest, TResponse> ReportBaseService;

        public IShowHideColumnService ShowHideColumnService { get; set; }

        protected virtual bool AutoLoadData => false;

        protected virtual bool ShowLoading => false;

        public virtual int MaxRangeDate => MobileSettingHelper.ViewReport;

        private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
        public virtual DateTime FromDate { get => fromDate; set => SetProperty(ref fromDate, value); }

        private DateTime minFromDate = DateTime.Today.AddYears(-1);
        public virtual DateTime MinFromDate { get => minFromDate; set => SetProperty(ref minFromDate, value); }

        private DateTime toDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        public virtual DateTime ToDate { get => toDate; set => SetProperty(ref toDate, value); }

        private DateTime maxToDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59).AddDays(1);
        public virtual DateTime MaxToDate { get => maxToDate; set => SetProperty(ref maxToDate, value); }

        private Company company;
        public Company Company { get => company; set => SetProperty(ref company, value); }

        private Vehicle vehicle;
        public Vehicle Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private TResponse listDataReport;
        public TResponse ListDataReport { get => listDataReport; set => SetProperty(ref listDataReport, value); }

        private bool hasData = true;
        public bool HasData { get => hasData; set => SetProperty(ref hasData, value); }

        private int pagedNext = MobileSettingHelper.ConfigPageNextReport;
        public virtual int PagedNext { get => pagedNext; set => SetProperty(ref pagedNext, value); }

        private int pageSize = MobileSettingHelper.ConfigPageSizeReport;
        public virtual int PageSize { get => pageSize; set => SetProperty(ref pageSize, value); }

        public virtual int ShowHideColumnTableID { get; set; }

        public virtual IDictionary<int, bool> ShowHideColumnDictionary => new Dictionary<int, bool>();

        public virtual string ReportTitle => MobileSettingHelper.ConfigTitleDefaultReport;

        public DelegateCommand SelectVehicleReportCommand { get; private set; }
        public ReportBaseViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            ReportBaseService = PrismApplicationBase.Current.Container.Resolve<TService>();
            SelectVehicleReportCommand = new DelegateCommand(SelectVehicleReport);
        }

        public ReportBaseViewModel(INavigationService navigationService, IReportBaseService<TRequest, TResponse> reportBaseService)
            : base(navigationService)
        {
            ReportBaseService = reportBaseService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehiclePlate)
            {
                Vehicle = vehiclePlate;

                OnVehicleSelected();
            }
            else if (parameters.TryGetValue(ParameterKey.Company, out Company company))
            {
                Company = company;

                OnCompanySelected();
            }
        }

        public override void OnPageAppearingFirstTime()
        {
            if (AutoLoadData)
                GetData();
        }

        public virtual bool ValidateInput(out string message)
        {
            message = string.Empty;

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

            if (MaxRangeDate != 0)
            {
                // check thời gian có vượt quá khoảng cho phép hay không
                if (ToDate.Subtract(FromDate).TotalDays > MaxRangeDate)
                {
                    message = string.Format(MobileResource.Common_Message_ErrorOverDateSearch, MaxRangeDate);
                    return false;
                }
            }

            return true;
        }

        public virtual void OnVehicleSelected()
        {
        }

        public virtual void OnCompanySelected()
        {
        }

        public virtual bool ValidateInput()
        {
            if (!ValidateInput(out string message))
            {
                DisplayMessage.ShowMessageInfo(message, 3000);
                return false;
            }
            return true;
        }

        public abstract TRequest SetInputData();

        public ICommand GetDataCommand => new Command(GetData);

        public virtual void GetData()
        {
            if (!ValidateInput())
            {
                OnGetDataFail();

                return;
            }

            ReportBaseService.Request = SetInputData();

            RunOnBackground(async () => { return await ReportBaseService.GetData(); }, showLoading: ShowLoading, onComplete: (result) =>
            {
                ListDataReport = result;

                if (ListDataReport == null)
                {
                    HasData = false;
                }
                else if (ListDataReport is IList list)
                {
                    HasData = list.Count > 0;
                }

                OnGetDataSuccess();
            }, onError: (ex) =>
            {
                OnGetDataFail();
            });
        }

        public virtual void OnGetDataSuccess()
        {
        }

        public virtual void OnGetDataFail()
        {
        }

        public ICommand GetMoreDataCommand => new Command(GetMoreData);

        public virtual void GetMoreData()
        {
            if (!ValidateInput())
            {
                return;
            }

            ReportBaseService.Request = SetInputData();

            RunOnBackground(async () => { return await ReportBaseService.GetMoreData(); }, onComplete: (result) =>
            {
                if (ListDataReport is IList list && result is IList more)
                {
                    foreach (var item in more)
                        list.Add(item);
                }

                OnGetMoreDataSuccess();
            }, onError: (ex) =>
            {
                OnGetMoreDataFail();
            });
        }

        public virtual void OnGetMoreDataSuccess()
        {
        }

        public virtual void OnGetMoreDataFail()
        {
        }

        public ICommand DateSelectedCommand => new Command(OnDateSelected);

        public virtual void OnDateSelected()
        {
        }

        public ICommand ExportExcelCommand => new Command(ExportExcel);

        public virtual void ExportExcel()
        {
        }

        public ICommand SaveShowHideComlumnCommand => new Command(SaveShowHideComlumn);

        public virtual void SaveShowHideComlumn()
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
    }
}