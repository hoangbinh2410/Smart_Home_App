using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Helpers;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleDetailViewModel : ViewModelBase
    {
        private readonly IDetailVehicleService detailVehicleService;

        public VehicleDetailViewModel(INavigationService navigationService,
            IDetailVehicleService detailVehicleService) : base(navigationService)
        {
            this.detailVehicleService = detailVehicleService;

            Title = MobileResource.DetailVehicle_Label_TilePage;

            InforBGT = new InforBGTResponse();
            MessageInforChargeMoney = string.Empty;
         
            _engineState = MobileResource.Common_Label_TurnOff;

            InitLoadCommand = new DelegateCommand(InitLoadCommandExecute);
            RefeshCommand = new DelegateCommand(InitLoadCommandExecute);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters?.GetValue<VehicleOnline>(ParameterKey.CarDetail) is VehicleOnline cardetail)
            {
                PK_VehicleID = (int)cardetail.VehicleId;
                VehiclePlate = cardetail.VehiclePlate;
            }
            InitLoadCommandExecute();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #region property

        // thông tin chung của xe không thay đổi
        public int PK_VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        private VehicleOnlineDetailViewModel inforDetail;
        public VehicleOnlineDetailViewModel InforDetail { get => inforDetail; set => SetProperty(ref inforDetail, value); }

        private string fuel;
        public string Fuel { get => fuel; set => SetProperty(ref fuel, value); }

        private string temperature;
        public string Temperature { get => temperature; set => SetProperty(ref temperature, value); }

        #region các thông tin chung của xe có sự thay đổi theo cấu hình

        private bool _isShowparkingVehicleNow;
        public bool IsShowParkingVehicleNow { get => _isShowparkingVehicleNow; set => SetProperty(ref _isShowparkingVehicleNow, value); }

        private bool _isShowparkingTurnOnVehecle;
        public bool IsShowParkingTurnOnVehecle { get => _isShowparkingTurnOnVehecle; set => SetProperty(ref _isShowparkingTurnOnVehecle, value); }

        #endregion các thông tin chung của xe có sự thay đổi theo cấu hình

        #region thông tin phí

        // nội dung câu thông báo thu phí
        private string _messageInforChargeMoney;

        public string MessageInforChargeMoney { get => _messageInforChargeMoney; set => SetProperty(ref _messageInforChargeMoney, value); }

        #endregion thông tin phí

        // thông tin BGT
        private InforBGTResponse _inforBGT;

        public InforBGTResponse InforBGT { get => _inforBGT; set => SetProperty(ref _inforBGT, value); }

        public string _engineState = string.Empty;

        public string EngineState
        {
            get { return _engineState; }
            set { SetProperty(ref _engineState, value); }
        }

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
                        UserId = userID,
                        vehicleID = PK_VehicleID,
                        vehiclePlate = VehiclePlate,
                    };
                    var response = await detailVehicleService.LoadAllInforVehicle(input);

                    if(response != null)
                    {
                        InforDetail = response;
                        Fuel = string.Format("{0}/{1}L", response.VehicleNl.NumberOfLiters, response.VehicleNl.Capacity);
                        Temperature = response.Temperature2 == null ? string.Format("[{0} °C]", response.Temperature) : string.Format("[{0} °C]", response.Temperature) + " - " + string.Format("[{0} °C]", response.Temperature2);

                        //Động cơ
                        EngineState = StateVehicleExtension.EngineState(new VehicleOnline
                        {
                            VehicleTime = response.VehicleTime,
                            IconCode = response.IconVehicle,
                            State = response.StatusEngineer.GetValueOrDefault(),
                            Velocity = response.VelocityGPS,
                            GPSTime = response.VehicleTime,
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

                    Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
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
            }
        }

        #endregion execute command
    }
}