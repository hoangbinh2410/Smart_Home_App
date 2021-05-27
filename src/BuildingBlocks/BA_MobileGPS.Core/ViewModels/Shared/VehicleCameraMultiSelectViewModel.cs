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
        private string listSelectedPlates { get; set; }
        public VehicleCameraMultiSelectViewModel(INavigationService navigationService, IStreamCameraService cameraService) : base(navigationService)
        {
            this.cameraService = cameraService;
            ConfirmCommand = new DelegateCommand(Confirm);
            SearchVehicleCommand = new DelegateCommand<TextChangedEventArgs>(SearchVehicle);
            TapListVehicleCommand = new DelegateCommand<object>(TapListVehicle);
        }

        #region Binding
        private List<CameraLookUpVehicleModel> listVehicle = new List<CameraLookUpVehicleModel>();
        public List<CameraLookUpVehicleModel> ListVehicle { get => listVehicle; set => SetProperty(ref listVehicle, value); }

        private bool hasVehicle;
        public bool HasVehicle { get => hasVehicle; set => SetProperty(ref hasVehicle, value); }
        public ICommand ConfirmCommand { get; }
        public ICommand TapListVehicleCommand { get; }
        public ICommand SearchVehicleCommand { get; }

        #endregion

        #region life cycle
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters.ContainsKey(ParameterKey.ListVehicleSelected)
                            && parameters.GetValue<string>(ParameterKey.ListVehicleSelected) is string list)
            {
                listSelectedPlates = list;
            }

            GetVehicleCamera();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

        }
        #endregion
        #region Function
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
                           where (b.HasVideo == true)
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

            if (!string.IsNullOrEmpty(listSelectedPlates))
            {
                foreach (var item in listcam)
                {
                    if (listSelectedPlates.Contains(item.VehiclePlate))
                    {
                        item.IsSelected = true;
                    }
                }
            }
          
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

        private void TapListVehicle(object obj)
        {
            if (obj != null && obj is Syncfusion.ListView.XForms.ItemTappedEventArgs args)
            {
                var seleted = (args.ItemData as CameraLookUpVehicleModel);
                if (seleted != null)
                {
                    if (seleted.IsSelected)
                    {
                        seleted.IsSelected = false;
                    }
                    else
                    {
                        seleted.IsSelected = true;
                    }
                }
            }
        }


        private void Confirm()
        {
            SafeExecute(async () =>
            {
                var listGroupSelected = ListVehicleOrigin.Where(x=>x.IsSelected).ToList();

                await NavigationService.GoBackAsync(parameters: new NavigationParameters
                        {
                            { ParameterKey.ListVehicleSelected,  listGroupSelected}
                        }, true, true);
            });
        }
        #endregion
    }
}
