using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;

using Prism.Commands;
using Prism.Navigation;

using Syncfusion.Data.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Extensions;
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
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
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
                    return await cameraService.GetListVehicleHasCamera(UserInfo.XNCode);
                },
                (lst) =>
                {
                    if (lst != null && lst.Count > 0)
                    {
                        StaticSettings.ListVehilceCamera = lst;
                        MappingCamera(lst);
                    }
                });
            }
        }

        private void MappingCamera(List<VehicleCamera> lstcamera)
        {
            try
            {
                var lstCamera = new List<CameraLookUpVehicleModel>();
                var lstvehicle = StaticSettings.ListVehilceOnline;
                foreach (var item in lstcamera.Where(x => x.HasVideo).ToList())
                {
                    var plate = item.VehiclePlate.Contains("_C") ? item.VehiclePlate.Replace("_C", "") : item.VehiclePlate;
                    var model = lstvehicle.FirstOrDefault(x => x.VehiclePlate.ToUpper() == plate.ToUpper());
                    if (model != null)
                    {
                        lstCamera.Add(new CameraLookUpVehicleModel()
                        {
                            VehiclePlate = item.VehiclePlate,
                            VehicleId = model.VehicleId,
                            GroupIDs = model.GroupIDs,
                            IconImage = model.IconImage,
                            Imei = model.Imei,
                            PrivateCode = item.VehiclePlate,
                            SortOrder = model.SortOrder,
                            VehicleTime = model.VehicleTime,
                            Velocity = model.Velocity,
                            Channel = item.Channel
                        });
                    }
                    else
                    {
                        var model_c = lstvehicle.FirstOrDefault(x => x.VehiclePlate.ToUpper() == item.VehiclePlate.ToUpper());
                        if (model_c != null)
                        {
                            lstCamera.Add(new CameraLookUpVehicleModel()
                            {
                                VehiclePlate = item.VehiclePlate,
                                VehicleId = model_c.VehicleId,
                                GroupIDs = model_c.GroupIDs,
                                IconImage = model_c.IconImage,
                                Imei = model_c.Imei,
                                PrivateCode = item.VehiclePlate,
                                SortOrder = model_c.SortOrder,
                                VehicleTime = model_c.VehicleTime,
                                Velocity = model_c.Velocity,
                                Channel = item.Channel
                            });
                        }
                    }
                }
                var listcam = lstCamera.Distinct().OrderByDescending(x => x.SortOrder).ToList();
                ListVehicleOrigin.Clear();
                ListVehicle.Clear();

                ListVehicleOrigin = listcam;

                ListVehicle = listcam;

                HasVehicle = ListVehicle.Count > 0;
            }
            catch (System.Exception)
            {
                return;
            }
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