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

        private ComboboxResponse _selectedLocationStart;
        public ComboboxResponse SelectedLocationStart { get => _selectedLocationStart; set => SetProperty(ref _selectedLocationStart, value); }

        private ComboboxResponse _selectedLocationEnd;
        public ComboboxResponse SelectedLocationEnd { get => _selectedLocationEnd; set => SetProperty(ref _selectedLocationEnd, value); }

        private TransportBusinessRequest _objRequest;
        private List<ComboboxRequest> _listLocation = new List<ComboboxRequest>();

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
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ListLocation") && parameters.GetValue<List<ComboboxRequest>>("ListLocation") is List<ComboboxRequest> listLocation)
            {
                _listLocation = listLocation;
            }
            if (parameters.ContainsKey("ObjRequest") && parameters.GetValue<TransportBusinessRequest>("ObjRequest") is TransportBusinessRequest objRequest)
            {
                _objRequest = objRequest;
                if(_objRequest != null)
                {
                    InPutValueRequest(_objRequest);
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

        /// <summary>Ghi nhớ dữ liệu lọc nâng cao</summary>
        /// <returns></returns>
        /// <Modified>
        /// Name     Date         Comments
        /// ducpv  29/11/2021   created
        /// </Modified>
        private void InPutValueRequest(TransportBusinessRequest objRequest)
        {
            if (!string.IsNullOrEmpty(objRequest.FromPositionIds) && !objRequest.FromPositionIds.Contains(","))
            {
                var objPosition = _listLocation.Where(x => x.Key.ToString() == objRequest.FromPositionIds).FirstOrDefault();
                SelectedLocationStart = new ComboboxResponse()
                {
                    Key = objPosition.Key,
                    Value = objPosition.Value,
                };
            }
            if (!string.IsNullOrEmpty(objRequest.ToPositionIds) && !objRequest.ToPositionIds.Contains(","))
            {
                var objPosition = _listLocation.Where(x => x.Key.ToString() == objRequest.ToPositionIds).FirstOrDefault();
                SelectedLocationEnd = new ComboboxResponse()
                {
                    Key = objPosition.Key,
                    Value = objPosition.Value,
                };
            }
            MinKm = objRequest.MinKm;
            MaxKm = objRequest.MaxKm;
            ISCheckAddress = objRequest.ISChecked;

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
                    { "dataCombobox", _listLocation },
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
                    { "dataCombobox", _listLocation },
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
            string fromPositionIds = "";
            string toPositionIds = "";
            if (SelectedLocationStart.Key != -1)
            {
                fromPositionIds = SelectedLocationStart.Key.ToString();
            }
            if (SelectedLocationEnd.Key != -1)
            {
                toPositionIds = SelectedLocationEnd.Key.ToString();
            }
            var objRequest = new TransportBusinessRequest()
            {
                FromPositionIds = fromPositionIds,
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
            if (MinKm < 0)
            {
                DisplayMessage.ShowMessageInfo("Giá trị km tối thiểu phải lơn hơn 0", 3000);
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
