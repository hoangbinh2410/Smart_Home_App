using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ModelViews;

using System.Collections.Generic;

namespace BA_MobileGPS.Core
{
    public class VehicleStatusHelper
    {
        public VehicleStatusHelper()
        {
        }

        public Dictionary<int, VehicleStatusViewModel> DictVehicleStatus = new Dictionary<int, VehicleStatusViewModel>()
        {
            {
                (int)VehicleStatusGroup.All,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.All,
                    Name = MobileResource.Online_Label_StatusCarAll,
                    Icon = "car_blue.png",
                    IsEnable = true,
                }
            },
            {
                (int)VehicleStatusGroup.VehicleDebtMoney,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.VehicleDebtMoney,
                    Name = MobileResource.Online_Label_StatusCarDebtMoney,
                    Icon = "car_grey.png",
                    IsEnable = MobileSettingHelper.IsUseVehicleDebtMoney
                }
            },
            {
                (int)VehicleStatusGroup.Moving,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.Moving,
                    Name = MobileResource.Online_Label_StatusCarMoving,
                    Icon = "car_blue.png",
                    IsEnable = true
                }
            },
            {
                (int)VehicleStatusGroup.Stoping,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.Stoping,
                    Name = MobileResource.Online_Label_StatusCarStoping,
                    Icon = "car_grey.png",
                    IsEnable = true
                }
            },
            {
                (int)VehicleStatusGroup.StopingOn,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.StopingOn,
                    Name = MobileResource.Online_Label_StatusCarStopOn,
                    Icon = "car_blue_grey.png",
                    IsEnable = App.AppType==AppType.VMS?false:true
                }
            },
            {
                (int)VehicleStatusGroup.EngineOn,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.EngineOn,
                    Name = MobileResource.Online_Label_StatusCarEngineOn,
                    Icon = "car_blue.png",
                    IsEnable = App.AppType==AppType.VMS?false:true
                }
            },
            {
                (int)VehicleStatusGroup.EngineOFF,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.EngineOFF,
                    Name = MobileResource.Online_Label_StatusCarEngineOff,
                    Icon = "car_grey.png",
                    IsEnable = App.AppType==AppType.VMS?false:true
                }
            },
            {
                (int)VehicleStatusGroup.OverVelocity,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.OverVelocity,
                    Name = MobileResource.Online_Label_StatusCarOverVelocity,
                    Icon = "car_red.png",
                    IsEnable = App.AppType==AppType.VMS?false:true
                }
            },
            {
                (int)VehicleStatusGroup.LostGPS,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.LostGPS,
                    Name = MobileResource.Online_Label_StatusCarLostGPS,
                    Icon = "ic_lost_gps.png",
                    IsEnable = true
                }
            },
            {
                (int)VehicleStatusGroup.LostGSM,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.LostGSM,
                    Name = MobileResource.Online_Label_StatusCarLostGSM,
                    Icon = "car_warn.png",
                    IsEnable = true
                }
            },
               {
                (int)VehicleStatusGroup.SatelliteError,
                new VehicleStatusViewModel
                {
                    ID = (int)VehicleStatusGroup.SatelliteError,
                    Name = MobileResource.Online_Label_StatusCarSatelliteError,
                    Icon = "ic_errorgps.png",
                    IsEnable = App.AppType==AppType.VMS
                }
            },
        };
    }
}