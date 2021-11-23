using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.Report.Station;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using System.Linq;
using BA_MobileGPS.Service.Report.TransportBusiness;
using BA_MobileGPS.Entities.RequestEntity.Report.TransportBusiness;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities.ResponeEntity.Report.TransportBusiness;

namespace BA_MobileGPS.Core.ViewModels.Report.TransportBusiness
{
    public class DetailedFilterPageViewModel : ViewModelBase
    {
        #region Property

        private Vehicle _vehicle;
        public Vehicle Vehicle
        {
            get => _vehicle;
            set => SetProperty(ref _vehicle, value);
        }

        private bool _isCheckAddress = false;
        public bool ISCheckAddress
        {
            get => _isCheckAddress;
            set => SetProperty(ref _isCheckAddress, value);
        }

        private double _minKm = 1;
        public double MinKm
        {
            get => _minKm;
            set => SetProperty(ref _minKm, value);
        }
        private double _maxKm = 1000;
        public double MaxKm
        {
            get => _maxKm;
            set => SetProperty(ref _maxKm, value);
        }

        private DateTime _fromDate;
        private DateTime _toDate;

        private ComboboxResponse _selectedLocationStart;
        public ComboboxResponse SelectedLocationStart { get => _selectedLocationStart; set => SetProperty(ref _selectedLocationStart, value); }

        private ComboboxResponse _selectedLocationEnd;
        public ComboboxResponse SelectedLocationEnd { get => _selectedLocationEnd; set => SetProperty(ref _selectedLocationEnd, value); }

        private List<ComboboxRequest> _listLocationStation = new List<ComboboxRequest>();

        public List<ComboboxRequest> ListLocationStation
        {
            get { return _listLocationStation; }
            set { SetProperty(ref _listLocationStation, value); }
        }

        #endregion Property

        #region Contructor

        private readonly IStationLocationService _iStationDetailsService;
        private readonly ITransportBusinessService _iTransportBusinessService;
        public ICommand ExcuteFilterCommand { get; }
        public ICommand PushLandmarkCommandStart { get; private set; }
        public ICommand PushLandmarkCommandEnd { get; private set; }
        public DetailedFilterPageViewModel(INavigationService navigationService, IStationLocationService iStationDetailsService,
                                           ITransportBusinessService iTransportBusinessService)
            : base(navigationService)
        {
            _iStationDetailsService = iStationDetailsService;
            _iTransportBusinessService = iTransportBusinessService;
            ExcuteFilterCommand = new DelegateCommand(ExcuteFilterClicked);
            PushLandmarkCommandStart = new DelegateCommand(ExecuteLandmarkComboboxStart);
            PushLandmarkCommandEnd = new DelegateCommand(ExecuteLandmarkComboboxEnd);
            EventAggregator.GetEvent<SelectComboboxEvent>().Subscribe(UpdateValueCombobox);
            //Xét default key = 0 => tất cả
            _selectedLocationStart = new ComboboxResponse()
            {
                Key = -1,
                Value = MobileResource.ReportSignalLoss_TitleStatus_All
            };
            _selectedLocationEnd = new ComboboxResponse()
            {
                Key = -1,
                Value = MobileResource.ReportSignalLoss_TitleStatus_All
            };
        }

        #endregion Contructor

        #region Lifecycle

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetListLocationStation();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
                {
                    Vehicle = vehicle;
                }
                if (parameters.ContainsKey("FromDate") && parameters.GetValue<DateTime>("FromDate") is DateTime fromDate)
                {
                    _fromDate = fromDate;
                }
                if (parameters.ContainsKey("ToDate") && parameters.GetValue<DateTime>("ToDate") is DateTime toDate)
                {
                    _toDate = toDate;
                }
            }
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
            EventAggregator.GetEvent<SelectComboboxEvent>().Unsubscribe(UpdateValueCombobox);
        }

        #endregion Lifecycle

        #region PrivateMethod

        /// <summary>Put dữ liệu cho combobox</summary>
        /// <returns>Chọn địa điểm</returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  23/11/2021   created
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

        public async void ExecuteLandmarkComboboxStart()
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
        public async void ExecuteLandmarkComboboxEnd()
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
                    { "ComboboxType", ComboboxType.Second },
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
        private void UpdateValueCombobox(ComboboxResponse param)
        {
            base.UpdateCombobox(param);
            if (param != null)
            {
                var dataResponse = param;
                if (dataResponse.ComboboxType == (Int16)ComboboxType.First)
                {
                    SelectedLocationStart = dataResponse;
                }
                if (dataResponse.ComboboxType == (Int16)ComboboxType.Second)
                {
                    SelectedLocationEnd = dataResponse;
                }
            }
        }
        private void ExcuteFilterClicked()
        {
            if(!CheckValidateKmInput())
            {
                return;
            }    
            string fromPositionIdHis = "";
            string toPositionIds = "";
            if (SelectedLocationStart.Key != -1)
            {
                fromPositionIdHis = SelectedLocationStart.Value;
            }
            if (SelectedLocationEnd.Key != -1)
            {
                fromPositionIdHis = SelectedLocationEnd.Value;
            }
            var objRequest = new TransportBusinessRequest()
            {
                CompanyID = CurrentComanyID,
                VehicleIDs = Vehicle.VehicleId.ToString(),
                FromDate = _fromDate,
                ToDate = _toDate,
                FromPositionIdHis = fromPositionIdHis,
                ToPositionIds = toPositionIds,
                MinKm = MinKm,
                MaxKm = MaxKm,
                ISChecked = ISCheckAddress
            };
            SafeExecute(async () =>
            {     
                var parameters = new NavigationParameters
                {
                    { "DataBusinessRequest", objRequest },
                };
                await NavigationService.GoBackAsync(parameters);
            });
        }
        private bool CheckValidateKmInput()
        {
            if (MinKm < 1)
            {
                DisplayMessage.ShowMessageInfo("Giá trị km tối thiểu phải lơn hơn 1", 3000);
                return false;
            }
            if (MinKm >= MaxKm)
            {
                DisplayMessage.ShowMessageInfo("Giá trị km tối thiểu phải nhỏ hơn giá trị km tối đa", 3000);
                return false;
            }
            return true;
        }

        #endregion PrivateMethod
    }
}
