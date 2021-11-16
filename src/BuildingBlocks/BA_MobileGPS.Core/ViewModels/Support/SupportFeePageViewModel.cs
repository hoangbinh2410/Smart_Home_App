using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.ResponeEntity.Support;
using BA_MobileGPS.Service;
using BA_MobileGPS.Service.IService.Support;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.Data.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SupportFeePageViewModel : ViewModelBase
    {
        #region Contructor

        private readonly IVehicleOnlineService vehicleOnlineService;
        public ICommand BackPageCommand { get; private set; }
        public ICommand PushMessageSuportPageCommand { get; private set; }
        private ISupportCategoryService _iSupportCategoryService;

        public SupportFeePageViewModel(INavigationService navigationService, ISupportCategoryService iSupportCategoryService, IVehicleOnlineService vehicleOnlineService) : base(navigationService)
        {
            this.vehicleOnlineService = vehicleOnlineService;
            BackPageCommand = new DelegateCommand(BackPage);
            Title = "Hỗ trợ khách hàng";
            PushMessageSuportPageCommand = new DelegateCommand(PushMessageSuportPage);
            _iSupportCategoryService = iSupportCategoryService;
        }
        #endregion Contructor

        #region Lifecycle
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (parameters != null)
            {
                if (parameters.ContainsKey("NoData") && parameters.GetValue<bool>("NoData") is bool para)
                {
                    MessageData = para;
                }
                else
                    if (parameters.ContainsKey("Support") && parameters.GetValue<SupportCategoryRespone>("Support") is SupportCategoryRespone objSupport)
                {
                    IsSupportDisconnectView = parameters.GetValue<bool>("BoolPage");
                    GetCollectionPage(objSupport);
                    Data = objSupport;
                    if (parameters.ContainsKey(ParameterKey.VehicleRoute) && parameters.GetValue<Vehicle>(ParameterKey.VehicleRoute) is Vehicle vehicle && !string.IsNullOrEmpty(vehicle.VehiclePlate))
                    {
                        Vehicle = vehicle;
                    }
                    else if (parameters.ContainsKey("ListVehicleSupport") && parameters.GetValue<List<VehicleOnline>>("ListVehicleSupport") is List<VehicleOnline> listvehicle)
                    {
                        foreach (var vhicleOnline in listvehicle)
                        {
                            Vehicle = new Vehicle()
                            {
                                VehicleId = vhicleOnline.VehicleId,
                                VehiclePlate = vhicleOnline.VehiclePlate,
                                PrivateCode = vhicleOnline.PrivateCode,
                                GroupIDs = vhicleOnline.GroupIDs,
                                Imei = vhicleOnline.Imei,
                                IconImage = vhicleOnline.IconImage,
                                VehicleTime = vhicleOnline.VehicleTime,
                                Velocity = vhicleOnline.Velocity,
                                SortOrder = vhicleOnline.SortOrder
                            };
                        }
                    }
                }
                else
                {
                    MessageData = true;
                }
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnDestroy()
        {
        }

        #endregion Lifecycle

        #region Property

        private bool messageData = false;

        public bool MessageData { get => messageData; set => SetProperty(ref messageData, value); }
        private bool IssupportDisconnectView = false;

        public bool IsSupportDisconnectView
        {
            get { return IssupportDisconnectView; }
            set
            {
                SetProperty(ref IssupportDisconnectView, value);
            }
        }

        public VehicleOnline ListVehicle;

        private Vehicle Vehicle;
        private SupportCategoryRespone data;

        public SupportCategoryRespone Data
        { get { return data; } set { SetProperty(ref data, value); } }

        public List<MessageSupportRespone> ListSupportContent { get; set; }

        private MessageSupportRespone _SupportContent = new MessageSupportRespone();

        public MessageSupportRespone SupportContent
        {
            get { return _SupportContent; }
            set { SetProperty(ref _SupportContent, value); }
        }

        #endregion Property

        #region PrivateMethod

        public void BackPage()
        {
            SafeExecute(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public void PushMessageSuportPage()
        {
            SafeExecute(async () =>
            {
                var parameters = new NavigationParameters
            {
                { "Support", Data },
                { ParameterKey.VehicleRoute, Vehicle }
            };
                await NavigationService.NavigateAsync("MessageSuportPage", parameters);
            });
        }

        private void GetCollectionPage(SupportCategoryRespone obj)
        {
            if (obj.MessageSupports != null && obj.MessageSupports.Count > 0)
            {
                MapData(obj.MessageSupports, obj);
            }
            else
            {
                RunOnBackground(async () =>
                {
                    return await _iSupportCategoryService.GetMessagesSupport(obj.ID);
                }, (result) =>
                {
                    MapData(result, obj);
                });
            }
        }
        private void MapData(List<MessageSupportRespone> lstData, SupportCategoryRespone obj)
        {
            if (lstData != null && lstData.Count > 0)
            {
                if (IsSupportDisconnectView)
                {
                    var model = lstData.Where(s => s.OrderNo == 0).FirstOrDefault();
                    model.Guides = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + model.Guides;
                    SupportContent = model;
                }
                else
                {
                    var model = lstData.Where(s => s.OrderNo == 1).FirstOrDefault();
                    model.Guides = "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,maximum-scale=1\" />" + model.Guides;
                    SupportContent = model;
                }
            }
        }

        #endregion PrivateMethod
    }
}