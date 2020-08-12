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
            IsVisibleInforChargeMoney = false;
            MessageInforChargeMoney = string.Empty;
            InforCommonVehicle = new InforCommonVehicleResponse
            {
                IconVehicle = "arrowcars_blue.png"
            };
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

        private InforCommonVehicleResponse _inforCommonVehicle;
        public InforCommonVehicleResponse InforCommonVehicle { get => _inforCommonVehicle; set => SetProperty(ref _inforCommonVehicle, value); }

        #region các thông tin chung của xe có sự thay đổi theo cấu hình

        private bool _isShowkilometAccumulated;
        public bool IsShowkilometAccumulated { get => _isShowkilometAccumulated; set => SetProperty(ref _isShowkilometAccumulated, value); }

        private bool _isShowcountStopVehicle;
        public bool IsShowcountStopVehicle { get => _isShowcountStopVehicle; set => SetProperty(ref _isShowcountStopVehicle, value); }

        private bool _isShowparkingVehicleNow;
        public bool IsShowParkingVehicleNow { get => _isShowparkingVehicleNow; set => SetProperty(ref _isShowparkingVehicleNow, value); }

        private bool _isShowparkingTurnOnVehecle;
        public bool IsShowParkingTurnOnVehecle { get => _isShowparkingTurnOnVehecle; set => SetProperty(ref _isShowparkingTurnOnVehecle, value); }

        private bool _isShowcountOpenDoor;
        public bool IsShowCountOpenDoor { get => _isShowcountOpenDoor; set => SetProperty(ref _isShowcountOpenDoor, value); }

        private bool _isShowcrane;
        public bool IsShowCrane { get => _isShowcrane; set => SetProperty(ref _isShowcrane, value); }

        private bool _isShowben;
        public bool IsShowBen { get => _isShowben; set => SetProperty(ref _isShowben, value); }

        private bool _isShowtemperature;
        public bool IsShowTemperature { get => _isShowtemperature; set => SetProperty(ref _isShowtemperature, value); }

        private bool _isShowfuel;
        public bool IsShowFuel { get => _isShowfuel; set => SetProperty(ref _isShowfuel, value); }

        private bool _isShowconcrete;
        public bool IsShowConcrete { get => _isShowconcrete; set => SetProperty(ref _isShowconcrete, value); }

        private bool _isShowDoor;
        public bool IsShowDoor { get => _isShowDoor; set => SetProperty(ref _isShowDoor, value); }

        private bool isMCExpried = false;
        public bool IsMCExpried { get => isMCExpried; set => SetProperty(ref isMCExpried, value); }

        #endregion các thông tin chung của xe có sự thay đổi theo cấu hình

        #region thông tin phí

        // ẩn hiện khối thu phí
        private bool _isVisibleInforChargeMoney;

        public bool IsVisibleInforChargeMoney { get => _isVisibleInforChargeMoney; set => SetProperty(ref _isVisibleInforChargeMoney, value); }

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

                    EngineState = StateVehicleExtension.EngineState(new VehicleOnline
                    {
                        VehicleTime = response.VehicleTime,
                        IconCode = response.IconVehicle,
                        State = response.StatusEngineer.GetValueOrDefault(),
                        Velocity = response.VelocityGPS,
                        GPSTime = response.VehicleTime,
                        IsEnableAcc = response.AccStatus.GetValueOrDefault()
                    });
                    // mapping dữ liệu về mảng cho hợp lý
                    var result = new DetailVehicleResponse();
                    InforCommonVehicle = new InforCommonVehicleResponse()
                    {
                        Address = response.Address,
                        VehiclePlate = response.VehiclePlate,
                        IconVehicle = IconCodeHelper.GetMarkerResource(new VehicleOnline
                        {
                            VehicleTime = response.VehicleTime,
                            IconCode = response.IconVehicle,
                            State = response.StatusEngineer.GetValueOrDefault(),
                            Velocity = response.VelocityGPS,
                            GPSTime = response.VehicleTime
                        }),
                        Speed = response.VelocityGPS,
                        Time = response.VehicleTime,
                        StatusEngineer = response.StatusEngineer.GetValueOrDefault(),
                        KilometInToDay = response.TotalKm.GetValueOrDefault(),
                        KilometAccumulated = response.KilometAccumulated,
                        CountStopVehicle = response.StopCount,
                        ParkingVehicleNow = response.StopTime,
                        ParkingTurnOnVehecle = response.ParkingTurnOnVehecle,
                        AirCondition = response.AirCondition.GetValueOrDefault(),
                        CountOpenDoor = response.VehicleHtn.DoorOpenCount,
                        Crane = response.Crane,
                        Ben = response.Ben,
                        Temperature = response.Temperature2 == null ? string.Format("[{0} °C]", response.Temperature) : string.Format("[{0} °C]", response.Temperature) + " - " + string.Format("[{0} °C]", response.Temperature2),
                        Fuel = string.Format("{0}/{1}L", response.VehicleNl.NumberOfLiters, response.VehicleNl.Capacity),
                        Concrete = response.Concrete,
                        MemoryStick = response.VehicleHtn.MemoryStatusLabel,
                        SpeedOverCount = response.SpeedOverCount,
                        MinutesOfDrivingTimeContinuous = response.VehicleHtn.MinutesOfDrivingTimeContinuous,
                        MinutesOfDrivingTimeInDay = response.VehicleHtn.MinutesOfDrivingTimeInDay,
                        LogfileProcessModules = response.LogfileProcessModules,
                        CancelErrorOptions = response.CancelErrorOptions,
                        Door = response.Door,
                        AccStatus = response.AccStatus,
                        ConcretePump = response.ConcretePump,
                        IsUseFuel = response.VehicleNl.IsUseFuel,
                        SIMPhoneNumber = response.SIMPhoneNumber,
                        IMEI = response.IMEI,
                        JoinSystemDate = response.JoinSystemDate,
                        MCExpried = response.MCExpried
                    };

                    //Ẩn hiện ngày hết hạn
                    if (InforCommonVehicle.MCExpried != null)
                    {
                        IsMCExpried = true;
                    }

                    // gán dữ liệu vào hàm inforBGT
                    InforBGT = new InforBGTResponse()
                    {
                        DriverID = 0,
                        DriverLicense = response.VehicleHtn.DriverLicense,
                        DriverName = response.VehicleHtn.DriverName,
                        PhoneNumber = response.VehicleHtn.MobileNumber,
                        DepartmentOfManagement = response.DepartmentManager,
                    };

                    // hiện thị lên là xe đang nợ cần đóng tiền thu phí
                    if (response.IsExpried)
                    {
                        IsVisibleInforChargeMoney = true;
                        MessageInforChargeMoney = string.Format(MobileResource.DetailVehicle_MessageFee, response.ExpriedDate.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        IsVisibleInforChargeMoney = false;
                        MessageInforChargeMoney = string.Empty;
                    }

                    // cấu hình ẩn hiện với các trường không có dữ liệu đổ ra
                    AutoShowHideParam(InforCommonVehicle);

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

        private void AutoShowHideParam(InforCommonVehicleResponse data)
        {
            try
            {
                // ẩn hiện với km tích luỹ
                IsShowkilometAccumulated = data.KilometAccumulated != null ? true : false;

                // ẩn hiện với dừng đỗ
                IsShowcountStopVehicle = data.CountStopVehicle != null ? true : false;

                if (InforCommonVehicle.Speed > 3)
                {
                    // ẩn hiện với đang đỗ
                    IsShowParkingVehicleNow = false;

                    // ẩn hiện với dừng đỗ nổ máy
                    IsShowParkingTurnOnVehecle = false;
                }
                else
                {
                    // ẩn hiện với đang đỗ
                    IsShowParkingVehicleNow = data.ParkingVehicleNow != null ? true : false;

                    // ẩn hiện với dừng đỗ nổ máy
                    IsShowParkingTurnOnVehecle = data.ParkingTurnOnVehecle != null ? true : false;
                }

                // ẩn hiện với Số lần mở cửa
                IsShowCountOpenDoor = data.CountOpenDoor != null ? true : false;

                // ẩn hiện với Cẩu
                IsShowCrane = data.Crane != null ? true : false;

                // ẩn hiện với Ben
                IsShowBen = data.Ben != null ? true : false;

                // ẩn hiện với Nhiệt độ
                IsShowTemperature = data.Temperature != null ? true : false;

                // ẩn hiện với Nhiên liệu
                IsShowFuel = data.IsUseFuel;

                // ẩn hiện với Bê tông
                IsShowConcrete = data.ConcretePump != null ? true : false;

                // ẩn hiện với Bê tông
                IsShowDoor = data.Door != null ? true : false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion execute command
    }
}