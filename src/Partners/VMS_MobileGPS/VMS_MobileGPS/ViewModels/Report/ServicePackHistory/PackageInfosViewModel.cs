using BA_MobileGPS.Core.ViewModels;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Navigation;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace VMS_MobileGPS.ViewModels
{
    public class PackageInfosViewModel : VMSBaseViewModel
    {
        private readonly IServicePackageHistoryService packageHistoryService;

        private List<ShipPackage> listData1;
        public List<ShipPackage> ListData1 { get => listData1; set => SetProperty(ref listData1, value); }

        private List<ShipPackage> listData2;
        public List<ShipPackage> ListData2 { get => listData2; set => SetProperty(ref listData2, value); }

        public PackageInfosViewModel(INavigationService navigationService, IServicePackageHistoryService packageHistoryService) : base(navigationService)
        {
            this.packageHistoryService = packageHistoryService;
        }

        public override Task InitializeAsync(INavigationParameters parameters)
        {
            GetShipPackages();

            return base.InitializeAsync(parameters);
        }

        private void GetShipPackages()
        {
            RunOnBackground(async () =>
            {
                return await packageHistoryService.GetShipPackages();
            },
            result =>
            {
                if (result.Success && result.Data != null)
                {
                    ListData1 = result.Data.FindAll(p => p.PackageTypeID == 1);
                    ListData2 = result.Data.FindAll(p => p.PackageTypeID == 2);
                }
            });
        }
    }
}