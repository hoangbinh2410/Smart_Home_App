using BA_MobileGPS;
using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resource;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;

using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VMS_MobileGPS.ViewModels
{
    public class VehicleDetailViewModel : ViewModelBase
    {
        private readonly IDetailVehicleService detailVehicleService;
        //public ICommand ShowInfoMessageBAPCommand { get; private set; }

        public VehicleDetailViewModel(INavigationService navigationService, IDetailVehicleService detailVehicleService)
            : base(navigationService)
        {
            this.detailVehicleService = detailVehicleService;

            Title = MobileResource.DetailVehicleVMS_Label_TilePage;
            // _messageBAP = string.Empty;
            shipDetailRespone = new ShipDetailRespone();

            //ShowInfoMessageBAPCommand = new DelegateCommand(ShowInfoMessageBAP);

            EventAggregator.GetEvent<ReceiveSendCarEvent>().Subscribe(OnReceiveSendCarSignalR);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters?.GetValue<VehicleOnline>(ParameterKey.CarDetail) is VehicleOnline cardetail)
            {
                VehiclePlate = cardetail.VehiclePlate;
                PK_VehicleID = (int)cardetail.VehicleId;

                GetShipOnlineDetail();
            }
        }

        public override void OnDestroy()
        {
            EventAggregator.GetEvent<ReceiveSendCarEvent>().Unsubscribe(OnReceiveSendCarSignalR);
        }

        #region property

        // thông tin chung của xe không thay đổi
        public int PK_VehicleID { get; set; }

        public string VehiclePlate { get; set; }

        //public string _messageBAP { get; set; }
        //public string MessageBAP
        //{
        //    get { return _messageBAP; }
        //    set
        //    {
        //        _messageBAP = value;

        //        RaisePropertyChanged(() => MessageBAP);
        //    }
        //}

        private ShipDetailRespone shipDetailRespone;

        public ShipDetailRespone ShipDetailRespone
        {
            get { return shipDetailRespone; }
            set
            {
                shipDetailRespone = value;

                RaisePropertyChanged();
            }
        }

        #endregion property

        #region command

        /// <summary>
        /// Nhận dữ liệu xe online
        /// </summary>
        /// <param name="e"></param>
        private void OnReceiveSendCarSignalR(VehicleOnline carInfo)
        {
            if (carInfo.VehicleId.Equals(PK_VehicleID))
            {
                var model = new ShipDetailRespone()
                {
                    Address = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(carInfo.Lat), GeoHelper.LongitudeToDergeeMinSec(carInfo.Lng)),
                    Latitude = carInfo.Lat,
                    Longtitude = carInfo.Lng,
                    Km = carInfo.TotalKm,
                    GPSTime = carInfo.GPSTime,
                    VelocityGPS = carInfo.Velocity,
                    IMEI = ShipDetailRespone.IMEI,
                    PortDeparture = ShipDetailRespone.PortDeparture,
                    ShipCaptainName = ShipDetailRespone.ShipCaptainName,
                    ShipCaptainPhoneNumber = ShipDetailRespone.ShipCaptainPhoneNumber,
                    ShipMembers = ShipDetailRespone.ShipMembers,
                    ShipOwnerName = ShipDetailRespone.ShipOwnerName,
                    ShipOwnerPhoneNumber = ShipDetailRespone.ShipOwnerPhoneNumber,
                    PrivateCode = ShipDetailRespone.PrivateCode
                };
                ShipDetailRespone = model;
            }
        }

        #endregion command

        #region execute command

        /// <summary>
        /// Load tất cả dữ liệu về thông tin chi tiết 1 xe
        /// </summary>
        /// <Modified>
        /// Name     Date         Comments
        /// hoangdt  3/15/2019   created
        /// </Modified>
        public async void GetShipOnlineDetail()
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
                    // gọi service để lấy dữ liệu trả về
                    var input = new ShipDetailRequest()
                    {
                        UserId = StaticSettings.User.UserId,
                        vehiclePlate = VehiclePlate,
                    };
                    var response = await detailVehicleService.GetShipDetail(input);
                    if (response != null)
                    {
                        response.Address = string.Join(", ", GeoHelper.LatitudeToDergeeMinSec(response.Latitude), GeoHelper.LongitudeToDergeeMinSec(response.Longtitude));

                        ShipDetailRespone = response;

                        //InitMessageBAP(response);
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
            }
        }

        #endregion execute command
    }
}