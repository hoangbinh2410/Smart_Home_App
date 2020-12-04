using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Utilities;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
{
    public class VehicleCameraMultiSelectViewModel : ViewModelBase
    {
        private CancellationTokenSource cts;
        private static List<CameraLookUpVehicleModel> ListVehicleOrigin = new List<CameraLookUpVehicleModel>();
        private readonly IStreamCameraService cameraService;

        public VehicleCameraMultiSelectViewModel(INavigationService navigationService, IStreamCameraService cameraService) : base(navigationService)
        {
            this.cameraService = cameraService;
            ConfirmCommand = new DelegateCommand(Confirm);
            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
        }


        private List<CameraLookUpVehicleModel> listVehicle = new List<CameraLookUpVehicleModel>();
        public List<CameraLookUpVehicleModel> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        private IList<object> selectedVehicles;
        public IList<object> SelectedVehicles
        {
            get { return selectedVehicles; }
            set { 
                SetProperty(ref selectedVehicles, value);
            }
        }

        private bool hasVehicle;
        public bool HasVehicle { get => hasVehicle; set => SetProperty(ref hasVehicle, value); }
        public ICommand ConfirmCommand { get; }
        public ICommand TapListVehicleCommand { get; }
        public ICommand SearchVehicleCommand { get;  }
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            GetVehicleCamera();
        }

        private void GetVehicleCamera()
        {
            if (StaticSettings.ListVehilceCamera != null && StaticSettings.ListVehilceCamera.Count > 0)
            {
                MappingCamera(StaticSettings.ListVehilceCamera);
            }
            else
            {
                RunOnBackground(async () =>
                {
                    return await cameraService.GetListVehicleCamera(UserInfo.XNCode);
                },
                (lst) =>
                {
                    if (lst != null && lst.Data?.Count > 0)
                    {
                        StaticSettings.ListVehilceCamera = lst.Data;
                        MappingCamera(lst.Data);
                    }
                });
            }
        }

        private void MappingCamera(List<StreamDevices> lstcamera)
        {
            var listcam = (from a in lstcamera
                           join b in StaticSettings.ListVehilceOnline on a.VehiclePlate.ToUpper() equals b.VehiclePlate.ToUpper()
                           select new CameraLookUpVehicleModel()
                           {
                               CameraChannels = a.CameraChannels?.Select(x => x.Channel).ToList(),
                               VehiclePlate = b.VehiclePlate,
                               VehicleId = b.VehicleId,
                               GroupIDs = b.GroupIDs,
                               IconImage = b.IconImage,
                               Imei = b.Imei,
                               PrivateCode = b.PrivateCode,
                               SortOrder = b.SortOrder,
                               VehicleTime = b.VehicleTime,
                               Velocity = b.Velocity
                           }).Distinct().OrderByDescending(x => x.SortOrder).ToList();
            ListVehicleOrigin.Clear();
            ListVehicle.Clear();

            ListVehicleOrigin = listcam;

            ListVehicle = listcam;

            HasVehicle = ListVehicle.Count > 0;
        }

        public void SearchVehicle(TextChangedEventArgs args)
        {
            if (cts != null)
                cts.Cancel(true);

            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                await Task.Delay(500, cts.Token);

                if (string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    return ListVehicleOrigin.ToList();
                }
                else
                {
                    return ListVehicleOrigin.FindAll(vg => vg.VehiclePlate != null && vg.VehiclePlate.UnSignContains(args.NewTextValue) || vg.PrivateCode != null && vg.PrivateCode.UnSignContains(args.NewTextValue));
                }
            }, cts.Token).ContinueWith(task => Device.BeginInvokeOnMainThread(() =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    var result = task.Result.OrderByDescending(x => x.SortOrder).ToList();
                    ListVehicle = result;

                    HasVehicle = ListVehicle.Count > 0;
                }
            }));
        }
      
       
        private void Confirm()
        {
            SafeExecute(async () =>
            {
                var listGroupSelected = selectedVehicles.Cast<CameraLookUpVehicleModel>().ToList();

                await NavigationService.GoBackAsync(parameters: new NavigationParameters
                        {
                            { ParameterKey.ListVehicleSelected,  listGroupSelected}
                        }, true, true);
            });
        }
    }
}
