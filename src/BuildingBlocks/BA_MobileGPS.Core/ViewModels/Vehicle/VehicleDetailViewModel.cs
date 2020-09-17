using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleDetailViewModel : ViewModelBase
    {
        private readonly IDetailVehicleService detailVehicleService;
        private readonly IGeocodeService geocodeService;
        public VehicleDetailViewModel(INavigationService navigationService, IGeocodeService geocodeService,
            IDetailVehicleService detailVehicleService) : base(navigationService)
        {
            this.geocodeService = geocodeService;
            this.detailVehicleService = detailVehicleService;

            Title = MobileResource.DetailVehicle_Label_TilePage;

            MessageInforChargeMoney = string.Empty;

            _engineState = MobileResource.Common_Label_TurnOff;

            InitLoadCommand = new DelegateCommand(InitLoadCommandExecute);
            RefeshCommand = new DelegateCommand(InitLoadCommandExecute);
            EventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(OnReceiveSendCarSignalR);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Subscribe(OnReLoadVehicleOnlineCarSignalR);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters?.GetValue<VehicleOnline>(ParameterKey.CarDetail) is VehicleOnline cardetail)
            {
                PK_VehicleID = (int)cardetail.VehicleId;
                VehiclePlate = cardetail.VehiclePlate;
                if (cardetail.CurrentAddress != null)
                {
                    Address = cardetail.CurrentAddress;
                }
                else
                {
                    Getaddress(cardetail.Lat.ToString(), cardetail.Lng.ToString());
                }
                InitLoadCommandExecute();
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Unsubscribe(OnReLoadVehicleOnlineCarSignalR);
        }

        #region property

        // thông tin chung của xe không thay đổi
        public int PK_VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        private Entities.VehicleDetailViewModel inforDetail;
        public Entities.VehicleDetailViewModel InforDetail { get => inforDetail; set => SetProperty(ref inforDetail, value); }

        private string fuel;
        public string Fuel { get => fuel; set => SetProperty(ref fuel, value); }

        private string temperature;
        public string Temperature { get => temperature; set => SetProperty(ref temperature, value); }

        #region thông tin phí

        // nội dung câu thông báo thu phí
        private string _messageInforChargeMoney;

        public string MessageInforChargeMoney { get => _messageInforChargeMoney; set => SetProperty(ref _messageInforChargeMoney, value); }

        #endregion thông tin phí

        // thông tin BGT

        public string _engineState = string.Empty;

        public string EngineState
        {
            get { return _engineState; }
            set { SetProperty(ref _engineState, value); }
        }

        private string address;

        public string Address { get => address; set => SetProperty(ref address, value); }

        private DateTime vehicleTime;

        public DateTime VehicleTime { get => vehicleTime; set => SetProperty(ref vehicleTime, value); }

        private int velocityGPS;

        public int VelocityGPS { get => velocityGPS; set => SetProperty(ref velocityGPS, value); }

        private float totalKm;

        public float TotalKm { get => totalKm; set => SetProperty(ref totalKm, value); }

        private int stopTime;

        public int StopTime { get => stopTime; set => SetProperty(ref stopTime, value); }

        #endregion property

        #region command

        public DelegateCommand InitLoadCommand { get; private set; }
        public ICommand RefeshCommand { get; private set; }

        #endregion command

        #region execute command

        /// <summary>
        /// Load tất cả dữ liệu về thông tin chi tiết 1 xe
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/15/2019   created
        /// </Modified>
        public async void InitLoadCommandExecute()
        {
            // gọi hàm dưới server để lấy dữ liệu về
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            try
            {
                if (IsConnected)
                {
                    Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("");
                    var userID = StaticSettings.User.UserId;
                    // Kiểm tra xem có chọn cty khác không
                    if (Settings.CurrentCompany != null && Settings.CurrentCompany.FK_CompanyID > 0)
                    {
                        userID = Settings.CurrentCompany.UserId;
                    }
                    // gọi service để lấy dữ liệu trả về
                    var input = new DetailVehicleRequest()
                    {
                        XnCode = StaticSettings.User.XNCode,
                        VehiclePlate = VehiclePlate,
                        CompanyId = StaticSettings.User.CompanyId
                    };
                    var response = await detailVehicleService.GetVehicleDetail(input);

                    if (response != null)
                    {
                        InforDetail = response;
                        if (response.VehicleNl == null)
                        {
                            InforDetail.VehicleNl = new VehicleNl();
                        }
                        else
                        {
                            Fuel = string.Format("{0}/{1}L", response.VehicleNl.NumberOfLiters, response.VehicleNl.Capacity);
                        }                                             
                        Temperature = response.Temperature2 == null ? string.Format("[{0} °C]", response.Temperature) : string.Format("[{0} °C]", response.Temperature) + " - " + string.Format("[{0} °C]", response.Temperature2);
                        VehicleTime = response.VehicleTime;
                        VelocityGPS = response.VelocityGPS;
                        TotalKm = response.TotalKm.GetValueOrDefault();
                        StopTime = response.StopTime.GetValueOrDefault();

                        //Động cơ
                        EngineState = StateVehicleExtension.EngineState(new VehicleOnline
                        {
                            VehicleTime = response.VehicleTime,
                            IconCode = response.IconVehicle,
                            State = response.StatusEngineer.GetValueOrDefault(),
                            Velocity = response.VelocityGPS,
                            GPSTime = response.GPSTime,
                            IsEnableAcc = response.AccStatus.GetValueOrDefault()
                        });

                        // hiện thị lên là xe đang nợ cần đóng tiền thu phí
                        if (response.IsExpried)
                        {
                            MessageInforChargeMoney = string.Format(MobileResource.DetailVehicle_MessageFee, response.ExpriedDate.ToString("dd/MM/yyyy"));
                        }
                        else
                        {
                            MessageInforChargeMoney = string.Empty;
                        }
                    }                   
                }
                else
                {
                    DisplayMessage.ShowMessageInfo(MobileResource.Common_ConnectInternet_Error, 5000);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
            }
        }

        private void OnReceiveSendCarSignalR(VehicleOnline carInfo)
        {
            if (carInfo == null || PK_VehicleID != carInfo.VehicleId)
            {
                return;
            }

            if (carInfo != null)
            {
                if (CompanyConfigurationHelper.VehicleOnlineAddressEnabled)
                {
                    if (StateVehicleExtension.IsMovingAndEngineON(carInfo))
                    {
                        Getaddress(carInfo.Lat.ToString(), carInfo.Lng.ToString());
                    }
                }
                ////////////////
                VehicleTime = carInfo.VehicleTime;
                VelocityGPS = carInfo.Velocity;
                TotalKm = (float)carInfo.TotalKm;
                StopTime = carInfo.StopTime;

                //Động cơ
                EngineState = StateVehicleExtension.EngineState(carInfo);

            }
        }

        private void OnReLoadVehicleOnlineCarSignalR(bool arg)
        {
            if (arg)
            {
                InitLoadCommandExecute();
            }
        }

        private void Getaddress(string lat, string lng)
        {
            try
            {
                Task.Run(async () =>
                {
                    return await geocodeService.GetAddressByLatLng(lat, lng);
                }).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        if (!string.IsNullOrEmpty(task.Result))
                        {
                            Address = task.Result;
                        }
                    }
                    else if (task.IsFaulted)
                    {
                        Logger.WriteError(MethodBase.GetCurrentMethod().Name, "Error");
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodInfo.GetCurrentMethod().Name, ex);
            }
        }

        #endregion execute command
    }
}