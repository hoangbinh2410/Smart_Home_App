using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Extensions;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace VMS_MobileGPS.ViewModels
{
    public class DistancePageViewModel : ViewModelBase
    {
        public DistancePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }

        private string distanceVehicleTitle;

        public string DistanceVehicleTitle { get => distanceVehicleTitle; set => SetProperty(ref distanceVehicleTitle, value); }

        private VehicleOnline vehicleSelect;
        public VehicleOnline VehicleSelect { get => vehicleSelect; set => SetProperty(ref vehicleSelect, value); }

        private ObservableCollection<DistanceResponse> listSearchData;
        public ObservableCollection<DistanceResponse> ListDataSearch { get => listSearchData; set => SetProperty(ref listSearchData, value); }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(ParameterKey.Vehicle) && parameters.GetValue<Vehicle>(ParameterKey.Vehicle) is Vehicle vehicle)
            {
                VehicleSelect = StaticSettings.ListVehilceOnline.Where(x => x.VehiclePlate == vehicle.VehiclePlate).FirstOrDefault();
                DistanceVehicleTitle = string.Format(MobileResource.Distance_Label_TitleDistance, vehicle.VehiclePlate);
            }

            else if (parameters.ContainsKey(ParameterKey.VehicleOnline) && parameters.GetValue<VehicleOnline>(ParameterKey.VehicleOnline) is VehicleOnline vehiclePlate)
            {
                VehicleSelect = vehiclePlate;
                DistanceVehicleTitle = string.Format(MobileResource.Distance_Label_TitleDistance, VehicleSelect.VehiclePlate);
            }
            ExcuteSearchData();
        }

        /// <summary>Đổ dữ liệu</summary>
        /// <Modified>
        /// Name     Date         Comments
        /// linhlv  1/10/2020   created
        /// </Modified>
        private void ExcuteSearchData()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (VehicleSelect != null)
                {
                    var tempListData = new ObservableCollection<DistanceResponse>();
                    var listVehilce = StaticSettings.ListVehilceOnline;
                    for (int i = 0; i < listVehilce.Count; i++)
                    {
                        if (VehicleSelect.VehiclePlate != listVehilce[i].VehiclePlate)
                        {
                            tempListData.Add(new DistanceResponse()
                            {
                                VehiclePlate = listVehilce[i].VehiclePlate,
                                VehicleTime = listVehilce[i].VehicleTime,
                                Velocity = listVehilce[i].Velocity,
                                Distance = StateVehicleExtension.IsLostGPS(listVehilce[i].GPSTime, listVehilce[i].VehicleTime)
                                            ? MobileResource.Distance_Label_LostGPS
                                            : CalculateDistanceHelper.ConvertDistanceMToNmi(CalculateDistanceHelper.CalculateDistance(VehicleSelect.Lng, VehicleSelect.Lat, listVehilce[i].Lng, listVehilce[i].Lat), 1).ToString().Replace(",", ".")
                            });
                        }
                    }
                    if (tempListData != null && tempListData.Count > 0)
                    {
                        ListDataSearch = new ObservableCollection<DistanceResponse>(tempListData);
                    }
                    else
                    {
                        ListDataSearch = new ObservableCollection<DistanceResponse>();
                    }
                }
                else
                {
                    ListDataSearch = new ObservableCollection<DistanceResponse>();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(MethodBase.GetCurrentMethod().Name, ex);
            }
            finally
            {
                IsBusy = false;
                HasData = ListDataSearch.Count > 0;
            }
        }
    }
}