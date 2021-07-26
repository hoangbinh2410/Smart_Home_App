using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class PackageCameraPageViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;

        public PackageCameraPageViewModel(INavigationService navigationService,
           IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            VehiclePlate = "29LD00791_C";
            GetPackageByXnPlate();
            //if (parameters?.GetValue<VehicleOnline>(ParameterKey.CarDetail) is VehicleOnline cardetail)
            //{
            //    TryExecute(async () =>
            //    {
            //        VehiclePlate = cardetail.VehiclePlate;
            //        GetPackageByXnPlate();
            //    });
            //}
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public override void OnDestroy()
        {
        }

        public string VehiclePlate { get; set; }

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
    }
}