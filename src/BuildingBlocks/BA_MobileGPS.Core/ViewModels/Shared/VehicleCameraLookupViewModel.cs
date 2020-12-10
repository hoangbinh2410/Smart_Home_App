using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using BA_MobileGPS.Utilities;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.Data.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;
using Exception = System.Exception;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace BA_MobileGPS.Core.ViewModels
{
    #region Property

    public class VehicleCameraLookupViewModel : ViewModelBase
    {
        private readonly IStreamCameraService cameraService;
        private CancellationTokenSource cts;
        private bool hasVehicle = true;
        public bool HasVehicle { get => hasVehicle; set => SetProperty(ref hasVehicle, value); }
       
        private static List<CameraLookUpVehicleModel> ListVehicleOrigin = new List<CameraLookUpVehicleModel>();

        private List<CameraLookUpVehicleModel> listVehicle = new List<CameraLookUpVehicleModel>();
        public List<CameraLookUpVehicleModel> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        #endregion Property

       

        public ICommand SearchVehicleCommand { get; private set; }

        public ICommand TapListVehicleCommand { get; }

        public VehicleCameraLookupViewModel(INavigationService navigationService, IStreamCameraService cameraService) : base(navigationService)
        {
            this.cameraService = cameraService;
            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
            TapListVehicleCommand = new DelegateCommand<ItemTappedEventArgs>(TapListVehicle);       
        }

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

        private void SearchVehicle(TextChangedEventArgs args)
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

        public void TapListVehicle(ItemTappedEventArgs args)
        {
            SafeExecute(async () =>
            {
                var selected = (args.ItemData as CameraLookUpVehicleModel);
                if (selected != null)
                {
                    var navigationPara = new NavigationParameters();
                    navigationPara.Add(ParameterKey.Vehicle, selected);
                    await NavigationService.GoBackAsync(navigationPara, useModalNavigation: true, true);
                }
            });
        }                
    }
}