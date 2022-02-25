using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using MOTO_MobileGPS.Constant;
using Prism.Navigation;
using System;
using System.Linq;

namespace MOTO_MobileGPS.ViewModels
{
    public class VehicleDetailPageViewModel : ViewModelBase
    {
        private readonly IGeocodeService geocodeService;

        private readonly IMotoDetailService motoDetailService;

        private readonly IMotoPropertiesService motoPropertiesService;

        private readonly IMotoSimMoneyService motoSimMoneyService;

        private string disconnectTimeStr;
        public string DisconnectTimeStr { get => disconnectTimeStr; set => SetProperty(ref disconnectTimeStr, value); }

        private MotoDetailViewModel motoDetail;
        public MotoDetailViewModel MotoDetail { get => motoDetail; set => SetProperty(ref motoDetail, value); }

        private string simMoney;
        public string SimMoney { get => simMoney; set => SetProperty(ref simMoney, value); }

        public VehicleDetailPageViewModel(INavigationService navigationService, IGeocodeService geocodeService,
            IMotoDetailService motoDetailService, IMotoPropertiesService motoPropertiesService, IMotoSimMoneyService motoSimMoneyService) : base(navigationService)
        {
            this.geocodeService = geocodeService;
            this.motoDetailService = motoDetailService;
            this.motoPropertiesService = motoPropertiesService;
            this.motoSimMoneyService = motoSimMoneyService;
            Title = MobileResource.DetailVehicle_Label_TilePage;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters?.GetValue<MotoDetailViewModel>(MotoParameterKey.MotoDetail) is MotoDetailViewModel motodetail)
            {
                MotoDetail = motodetail;
                if (MotoDetail.VehiclePlate != null)
                {
                    GetAddInfo();
                    var vehicleID = StaticSettings.ListVehilceOnline.Where(x => x.VehiclePlate == motodetail.VehiclePlate).FirstOrDefault().VehicleId;
                    if (vehicleID > 0)
                    {
                        GetMoney(vehicleID);
                    }
                }
            }
            else if (parameters?.GetValue<VehicleOnline>(ParameterKey.CarDetail) is VehicleOnline cardetail)
            {
                GetMotoProperties(cardetail);
            }
        }

        private void GetMotoProperties(VehicleOnline carinfo)
        {
            RunOnBackground(async () =>
            {
                return await motoPropertiesService.GetMotoProperties(StaticSettings.User.XNCode, carinfo.VehiclePlate);
            }, (temp) =>
            {
                if (temp != null && temp.Data != null)
                {
                    MotoStaticSettings.MotoProperties = temp.Data.GetXMPropertiesResult;

                    GetDetailMoto(carinfo);
                }
                else
                {
                    MotoStaticSettings.MotoProperties = new MotoPropertiesViewModel();
                }
            });
        }

        private void GetDetailMoto(VehicleOnline carinfo)
        {
            RunOnBackground(async () =>
            {
                return await motoDetailService.GetMotoDetail(StaticSettings.User.XNCode, carinfo.VehiclePlate);
            }, (response) =>
            {
                if (response != null && response.Data !=null)
                {
                    MotoDetail = response.Data.MotoDetailResult;
                    MotoDetail.DevicePhone = MotoStaticSettings.MotoProperties.DevicePhoneNumber;

                    Getaddress(carinfo.Lat.ToString(), carinfo.Lng.ToString());

                    GetAddInfo();

                    GetMoney(carinfo.VehicleId);
                }
                else
                {
                    MotoDetail = new MotoDetailViewModel();
                }
            });
        }

        private void Getaddress(string lat, string lng)
        {
            RunOnBackground(async () =>
            {
                return await geocodeService.GetAddressByLatLng(lat, lng);
            }, (response) =>
            {
                if (!string.IsNullOrEmpty(response))
                {
                    MotoDetail.Address = response;
                }
            });
        }

        private void GetAddInfo()
        {
            if (MotoDetail.DisconnectTime.ToShortDateString() == DateTime.Parse("01/01/2000").ToShortDateString())
            {
                DisconnectTimeStr = string.Empty;
            }
            else
            {
                DisconnectTimeStr = DateTimeHelper.FormatDateTime(MotoDetail.DisconnectTime);
            }
        }

        private void GetMoney(long vehicleID)
        {
            RunOnBackground(async () =>
            {
                return await motoSimMoneyService.GetSimMoney(vehicleID);
            }, (temp) =>
            {
                if (temp != null && temp.Data != null)
                {
                    SimMoney = String.Format("{0:0,0}", temp.Data.Money) + " VNĐ";
                }
                else
                {
                    SimMoney = "0 VNĐ";
                }
            });
        }
    }
}