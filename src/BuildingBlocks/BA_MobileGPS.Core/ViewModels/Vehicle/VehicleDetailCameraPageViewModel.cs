﻿using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Utilities.Helpers;
using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleDetailCameraPageViewModel : ViewModelBase
    {
        private readonly IDetailVehicleService detailVehicleService;
        private readonly IPapersInforService papersInforService;
        private readonly IGeocodeService geocodeService;
        private readonly IStreamCameraService streamCameraService;
        public ICommand GotoCameraPageComamnd { get; }
        public ICommand SelectedMenuCommand { get; }

        public VehicleDetailCameraPageViewModel(INavigationService navigationService, IGeocodeService geocodeService,
            IDetailVehicleService detailVehicleService, IPapersInforService papersInforService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.geocodeService = geocodeService;
            this.detailVehicleService = detailVehicleService;
            this.papersInforService = papersInforService;
            this.streamCameraService = streamCameraService;
            Title = MobileResource.DetailVehicle_Label_TilePage;

            MessageInforChargeMoney = string.Empty;

            _engineState = MobileResource.Common_Label_TurnOff;
            IsShowCoordinates = CompanyConfigurationHelper.IsShowCoordinates;

            RefeshCommand = new DelegateCommand(RefeshData);
            EventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(OnReceiveSendCarSignalR);
            EventAggregator.GetEvent<OnReloadVehicleOnline>().Subscribe(OnReLoadVehicleOnlineCarSignalR);
            GotoCameraPageComamnd = new DelegateCommand<object>(GotoCameraPage);
            SelectedMenuCommand = new Command<MenuItem>(SelectedMenu);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters?.GetValue<VehicleOnline>(ParameterKey.CarDetail) is VehicleOnline cardetail)
            {
                TryExecute(async () =>
                {
                    PK_VehicleID = (int)cardetail.VehicleId;
                    VehiclePlate = cardetail.VehiclePlate;
                    PrivateCode = cardetail.PrivateCode;
                    if (cardetail.CurrentAddress != null)
                    {
                        Address = cardetail.CurrentAddress;
                    }
                    else
                    {
                        Getaddress(cardetail.Lat.ToString(), cardetail.Lng.ToString());
                    }
                    Coordinates = cardetail.Lat.ToString().Replace(",", ".") + ", " + cardetail.Lng.ToString().Replace(",", ".");
                    GetVehicleDetail();
                    IsCameraEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView);
                    GetPackageByXnPlate();
                    GetCameraInfor();
                    // Nếu có quyền hiển thị ngày đăng kiểm => hiển thị ngày bảo hiểm
                    // Thông tin k cần update liên tục => vứt ở đây
                    if (CompanyConfigurationHelper.IsShowDateOfRegistration)
                    {
                        InsuranceDate = await papersInforService.GetLastPaperDateByVehicle(StaticSettings.User.CompanyId, PK_VehicleID, PaperCategoryTypeEnum.Insurrance);
                        DateOfRegistration = await papersInforService.GetLastPaperDateByVehicle(StaticSettings.User.CompanyId, PK_VehicleID, PaperCategoryTypeEnum.Registry);
                        if (insuranceDate != null || dateOfRegistration != null)
                        {
                            ShowPaperInfor = true;
                        }
                    }
                });
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

        private bool fieldName;

        public bool IsCameraEnable
        {
            get { return fieldName; }
            set
            {
                SetProperty(ref fieldName, value);
                RaisePropertyChanged();
            }
        }

        // thông tin chung của xe không thay đổi
        public int PK_VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        public string PrivateCode { get; set; }

        private Entities.VehicleDetailViewModel inforDetail;

        public Entities.VehicleDetailViewModel InforDetail
        {
            get { return inforDetail; }
            set
            {
                SetProperty(ref inforDetail, value);
                RaisePropertyChanged();
            }
        }

        private string fuel;
        public string Fuel { get => fuel; set => SetProperty(ref fuel, value); }

        private double fuelProgress;
        public double FuelProgress { get => fuelProgress; set => SetProperty(ref fuelProgress, value); }

        private string temperature;
        public string Temperature { get => temperature; set => SetProperty(ref temperature, value); }

        public bool isShowCoordinates;
        public bool IsShowCoordinates { get => isShowCoordinates; set => SetProperty(ref isShowCoordinates, value); }

        private string coordinates;
        public string Coordinates { get => coordinates; set => SetProperty(ref coordinates, value); }

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

        private bool isFuelVisible;

        public bool IsFuelVisible
        {
            get { return isFuelVisible; }
            set
            {
                SetProperty(ref isFuelVisible, value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// KM trong tháng
        /// </summary>
        private float kmInMonth;

        public float KmInMonth
        {
            get { return kmInMonth; }
            set { SetProperty(ref kmInMonth, value); }
        }

        private DateTime? insuranceDate;

        public DateTime? InsuranceDate
        {
            get { return insuranceDate; }
            set { SetProperty(ref insuranceDate, value); }
        }

        private DateTime? dateOfRegistration;

        public DateTime? DateOfRegistration
        {
            get { return dateOfRegistration; }
            set { SetProperty(ref dateOfRegistration, value); }
        }

        private bool showPaperInfor;

        public bool ShowPaperInfor
        {
            get { return showPaperInfor; }
            set { SetProperty(ref showPaperInfor, value); }
        }

        private ServerServiceInfo serverServiceInfo = new ServerServiceInfo();

        public ServerServiceInfo ServerServiceInfo
        {
            get { return serverServiceInfo; }
            set { SetProperty(ref serverServiceInfo, value); }
        }

        private CloudServiceInfoInfo cloudServiceInfo = new CloudServiceInfoInfo();

        public CloudServiceInfoInfo CloudServiceInfo
        {
            get { return cloudServiceInfo; }
            set { SetProperty(ref cloudServiceInfo, value); }
        }

        private SimCardServiceInfo simCardServiceInfo = new SimCardServiceInfo();

        public SimCardServiceInfo SimCardServiceInfo
        {
            get { return simCardServiceInfo; }
            set { SetProperty(ref simCardServiceInfo, value); }
        }

        private StreamDevices streamDevices = new StreamDevices();

        public StreamDevices StreamDevices
        {
            get { return streamDevices; }
            set { SetProperty(ref streamDevices, value); }
        }

        private Coreboard coreboard = new Coreboard();

        public Coreboard Coreboard
        {
            get { return coreboard; }
            set { SetProperty(ref coreboard, value); }
        }

        private StorageDevices storageDevices = new StorageDevices();

        public StorageDevices StorageDevices
        {
            get { return storageDevices; }
            set { SetProperty(ref storageDevices, value); }
        }

        private double storageProgress;
        public double StorageProgress { get => storageProgress; set => SetProperty(ref storageProgress, value); }

        private string storageValue = "0/0 GB";
        public string StorageValue { get => storageValue; set => SetProperty(ref storageValue, value); }

        private string channelString;

        public string ChannelString { get => channelString; set => SetProperty(ref channelString, value); }

        private string channelActive;

        public string ChannelActive { get => channelActive; set => SetProperty(ref channelActive, value); }

        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                SetProperty(ref menuItems, value);
                RaisePropertyChanged();
            }
        }

        #endregion property

        #region command

        public ICommand RefeshCommand { get; private set; }

        #endregion command

        #region execute command

        private void InitMenuItems()
        {
            var list = new List<MenuItem>();
            list.Add(new MenuItem
            {
                Title = "Video",
                Icon = "ic_videolive.png",
                Url = "NavigationPage/CameraManagingPage",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });
            list.Add(new MenuItem
            {
                Title = "Hình Ảnh",
                Icon = "ic_cameraonline.png",
                Url = "NavigationPage/ImageManagingPage",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingOnlineByImagesView),
            });
            list.Add(new MenuItem
            {
                Title = "Nhiên liệu",
                Icon = "ic_fuel.png",
                Url = "NavigationPage/ChartFuelReportPage",
                IsEnable = IsFuelVisible == true && CheckPermision((int)PermissionKeyNames.ShowFuelChartOnline) ? true : false,
            });
            list.Add(new MenuItem
            {
                Title = "Nhiệt độ",
                Icon = "ic_temperature.png",
                Url = "NavigationPage/ReportTableTemperature",
                IsEnable = !string.IsNullOrEmpty(Temperature) && CheckPermision((int)PermissionKeyNames.ReportTemperatureView) ? true : false,
            });
            MenuItems = list.Where(x => x.IsEnable == true).ToObservableCollection();
        }

        /// <summary>
        /// Load tất cả dữ liệu về thông tin chi tiết 1 xe
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/15/2019   created
        /// </Modified>
        public void GetVehicleDetail()
        {
            // gọi service để lấy dữ liệu trả về
            var input = new DetailVehicleRequest()
            {
                XnCode = StaticSettings.User.XNCode,
                VehiclePlate = VehiclePlate,
                CompanyId = StaticSettings.User.CompanyId
            };
            RunOnBackground(async () =>
            {
                return await detailVehicleService.GetVehicleDetail(input);
            }, (response) =>
            {
                if (response != null)
                {
                    InforDetail = response;
                    if (response.VehicleNl != null)
                    {
                        IsFuelVisible = response.VehicleNl.IsUseFuel;
                        Fuel = string.Format("{0}/{1}L", response.VehicleNl.NumberOfLiters, response.VehicleNl.Capacity);
                        FuelProgress = (response.VehicleNl.NumberOfLiters / response.VehicleNl.Capacity) * 100;
                    }
                    else
                    {
                        IsFuelVisible = false;
                    }
                    if (response.Temperature != null)
                    {
                        if (response.Temperature2 == null)
                        {
                            Temperature = string.Format("[{0} °C]", response.Temperature);
                        }
                        else
                        {
                            Temperature = string.Format("[{0} °C]", response.Temperature) + " - " + string.Format("[{0} °C]", response.Temperature2);
                        }
                    }
                    VehicleTime = response.VehicleTime;
                    VelocityGPS = response.VelocityGPS;
                    TotalKm = response.TotalKm.GetValueOrDefault();
                    StopTime = response.StopTime.GetValueOrDefault();

                    //Động cơ
                    EngineState = StateVehicleExtension.EngineState(new VehicleOnline
                    {
                        VehicleTime = response.VehicleTime,
                        State = response.StatusEngineer.GetValueOrDefault(),
                        Velocity = response.VelocityGPS,
                        GPSTime = response.GPSTime,
                        IsEnableAcc = response.AccStatus.GetValueOrDefault()
                    });
                    Address = response.Address;
                    // hiện thị lên là xe đang nợ cần đóng tiền thu phí
                    if (response.IsExpried)
                    {
                        MessageInforChargeMoney = string.Format(MobileResource.DetailVehicle_MessageFee, response.ExpriedDate.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        MessageInforChargeMoney = string.Empty;
                    }

                    if (CompanyConfigurationHelper.ShowKmInMonth)
                    {
                        KmInMonth = response.KmInMonth;
                    }
                    else
                    {
                        KmInMonth = 0;
                    }

                    InitMenuItems();
                }
            });
        }

        private void OnReceiveSendCarSignalR(VehicleOnline carInfo)
        {
            if (carInfo == null || PK_VehicleID != carInfo.VehicleId)
            {
                return;
            }

            if (carInfo != null)
            {
                GetVehicleDetail();

                Coordinates = carInfo.Lat.ToString().Replace(",", ".") + ", " + carInfo.Lng.ToString().Replace(",", ".");
            }
        }

        private void OnReLoadVehicleOnlineCarSignalR(bool arg)
        {
            if (arg)
            {
                GetVehicleDetail();
            }
        }

        private void Getaddress(string lat, string lng)
        {
            try
            {
                Task.Run(async () =>
                {
                    return await geocodeService.GetAddressByLatLng(CurrentComanyID, lat, lng);
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

        #region Camera

        private void RefeshData()
        {
            SafeExecute(() =>
            {
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().DisplayProgress("");
                GetVehicleDetail();
                GetCameraInfor();
                GetPackageByXnPlate();
                Xamarin.Forms.DependencyService.Get<IHUDProvider>().Dismiss();
            });
        }

        private async void GotoCameraPage(object obj)
        {
            var param = obj.ToString();
            await NavigationService.GoBackAsync(parameters: new NavigationParameters
                        {
                            { "pagetoNavigation",  param}
                        }, true, true);
        }

        private void SelectedMenu(MenuItem obj)
        {
            if (obj == null) return;
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters();
                if (obj.Url == "NavigationPage/CameraManagingPage")
                {
                    parameters.Add(ParameterKey.Vehicle, new CameraLookUpVehicleModel() { VehicleId = PK_VehicleID, VehiclePlate = VehiclePlate, PrivateCode = PrivateCode });
                }
                if (obj.Url == "NavigationPage/ChartFuelReportPage")
                {
                    parameters.Add(ParameterKey.Vehicle, new Vehicle() { VehicleId = PK_VehicleID, VehiclePlate = VehiclePlate, PrivateCode = PrivateCode });
                }
                else
                {
                    parameters.Add(ParameterKey.VehicleRoute, new Vehicle() { VehicleId = PK_VehicleID, VehiclePlate = VehiclePlate, PrivateCode = PrivateCode });
                }
                await NavigationService.NavigateAsync(obj.Url, parameters, useModalNavigation: true, true);
            });
        }

        private void GetPackageByXnPlate()
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetPackageByXnPlate(new PackageBACameraRequest()
                {
                    XNCode = UserInfo.XNCode,
                    LstPlate = new List<string>() { VehiclePlate }
                });
            }, (result) =>
            {
                if (result != null && result.Data != null)
                {
                    var model = result.Data.FirstOrDefault(x => x.VehiclePlate == VehiclePlate);
                    if (model != null)
                    {
                        if (model.ServerServiceInfoEnt != null)
                        {
                            ServerServiceInfo = model.ServerServiceInfoEnt;
                        }
                        if (model.CloudServiceInfoInfoEnt != null)
                        {
                            CloudServiceInfo = model.CloudServiceInfoInfoEnt;
                        }
                        if (model.SimCardServiceInfoEnt != null)
                        {
                            SimCardServiceInfo = model.SimCardServiceInfoEnt;
                        }
                    }
                }
            });
        }

        private void GetCameraInfor()
        {
            RunOnBackground(async () =>
            {
                return await streamCameraService.GetDevicesStatus(ConditionType.BKS, VehiclePlate);
            }, (result) =>
            {
                if (result != null && result.Data != null)
                {
                    var model = result.Data.FirstOrDefault(x => x.VehiclePlate == VehiclePlate);
                    if (model != null)
                    {
                        StreamDevices = model;
                        if (model.CameraChannels != null && model.CameraChannels.Count > 0)
                        {
                            ChannelString = string.Join(",", model.CameraChannels.Select(x => x.Channel));
                            var channelActive = model.CameraChannels.Where(x => x.HasCamera == true).ToList();
                            if (channelActive != null && channelActive.Count > 0)
                            {
                                ChannelActive = string.Join(",", channelActive.Select(x => x.Channel));
                            }
                        }
                        if (model.Coreboard != null)
                        {
                            Coreboard = model.Coreboard;
                        }
                        if (model.StorageDevices != null)
                        {
                            var storage = model.StorageDevices.FirstOrDefault(x => x.IsInserted == true && x.TotalSize > 0);
                            if (storage != null)
                            {
                                StorageDevices = storage;
                                if (storage.TotalSize > 0)
                                {
                                    var totalSize = Math.Round(StorageConverter.Convert(Differential.ByteToGiga, storage.TotalSize), 1);
                                    var freeSize = Math.Round(StorageConverter.Convert(Differential.ByteToGiga, storage.FreeSize), 1);
                                    var useSize = totalSize - freeSize;
                                    StorageProgress = (useSize / totalSize) * 100;
                                    StorageValue = useSize + "/" + totalSize + " GB";
                                }
                            }
                        }
                    }
                }
            });
        }

        #endregion Camera
    }
}