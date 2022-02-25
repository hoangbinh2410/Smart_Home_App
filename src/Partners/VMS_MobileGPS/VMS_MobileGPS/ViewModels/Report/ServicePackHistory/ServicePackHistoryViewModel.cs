using BA_MobileGPS.Core;
using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities;
using Prism.Commands;
using Prism.Navigation;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VMS_MobileGPS.ViewModels
{
    public class ServicePackHistoryViewModel : ReportBaseViewModel<IServicePackageHistoryService, ServicePackHistoryRequest, List<ServicePackHistory>>
    {
        private readonly IServicePackageHistoryService packageHistoryService;

        public override int MaxRangeDate => 365;

        private ServicePackageInfo currentServicePack;
        public ServicePackageInfo CurrentServicePack { get => currentServicePack; set => SetProperty(ref currentServicePack, value); }

        public ICommand ViewPackageInfosCommand { get; private set; }

        public ServicePackHistoryViewModel(INavigationService navigationService, IServicePackageHistoryService packageHistoryService) : base(navigationService, packageHistoryService)
        {
            this.packageHistoryService = packageHistoryService;

            FromDate = DateTime.Today.Subtract(TimeSpan.FromDays(90));
            ToDate = DateTime.Today.Add(TimeSpan.FromDays(90));

            ViewPackageInfosCommand = new DelegateCommand(ViewServicePackHistory);
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(ParameterKey.ShipDetail, out ShipDetailRespone vehicle))
            {
                Vehicle = new Vehicle()
                {
                    PrivateCode = vehicle.PrivateCode,
                    VehiclePlate = vehicle.PrivateCode
                };

                OnVehicleSelected();
            }

            return base.InitializeAsync(parameters);
        }

        private void GetCurrentServicePack()
        {
            RunOnBackground(async () =>
            {
                return await packageHistoryService.GetCurrentServicePack(new { UserInfo.XNCode, Vehicle.VehiclePlate });
            },
            result =>
            {
                CurrentServicePack = default;

                if (String.IsNullOrEmpty(result.Data.VehiclePlate))
                {
                    CurrentServicePack = result.Data;
                }
            });
        }

        public override void OnDateSelected()
        {
            if (Vehicle != null)
            {
                GetData();
            }
        }

        public override void OnVehicleSelected()
        {
            if (Vehicle != null)
            {
                GetData();
                GetCurrentServicePack();
            }
        }

        public override ServicePackHistoryRequest SetInputData()
        {
            return new ServicePackHistoryRequest
            {
                XNCode = UserInfo.XNCode,
                VehiclePlate = Vehicle.VehiclePlate,
                DateStart = FromDate,
                DateEnd = ToDate
            };
        }


        private void ViewServicePackHistory()
        {
            SafeExecute(async () =>
            {
                await NavigationService.NavigateAsync("PackageInfosPage", null,useModalNavigation: false,true);
            });
        }


        //private async void ViewPackageInfos(object sender, TaskCompletionSource<bool> tcs)
        //{
        //    await NavigationService.NavigateAsync(PageNames.PackageInfosPage.ToString()).ConfigureAwait(false);
        //    tcs.TrySetResult(true);
        //}
    }
}