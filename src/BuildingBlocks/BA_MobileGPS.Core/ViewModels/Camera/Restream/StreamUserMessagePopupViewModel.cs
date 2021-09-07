using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Entities.Enums;
using BA_MobileGPS.Service;
using BA_MobileGPS.Utilities.Extensions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class StreamUserMessagePopupViewModel : ViewModelBase
    {
        private readonly IStreamCameraService _streamCameraService;

        public StreamUserMessagePopupViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            Title = MobileResource.Common_Label_Notification;
            _streamCameraService = streamCameraService;
            _isShowUser = false;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters != null)
            {
                if (parameters.TryGetValue("StreamUser", out Tuple<Vehicle, List<StreamUserRequest>> obj))
                {
                    Vehicle = obj.Item1;
                    StreamUserRequest = obj.Item2;
                    ShowMessage(obj);
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private Vehicle vehicle = new Vehicle();
        public Vehicle Vehicle { get => vehicle; set => SetProperty(ref vehicle, value); }

        private List<StreamUserRequest> streamkUserRequest = new List<StreamUserRequest>();
        public List<StreamUserRequest> StreamUserRequest { get => streamkUserRequest; set => SetProperty(ref streamkUserRequest, value); }

        private string _massage;

        public string Message
        {
            get { return _massage; }
            set { SetProperty(ref _massage, value); }
        }

        private string _lstUser;

        public string LstUser
        {
            get { return _lstUser; }
            set { SetProperty(ref _lstUser, value); }
        }

        private bool _isShowUser;

        public bool IsShowUser
        {
            get { return _isShowUser; }
            set { SetProperty(ref _isShowUser, value); }
        }

        private void ShowMessage(Tuple<Vehicle, List<StreamUserRequest>> obj)
        {
            Message = string.Format(MobileResource.Camera_Message_DeviceStreamingErrorDetail,
                   obj.Item1.PrivateCode);
            string listUser = string.Empty;
            foreach (var item in obj.Item2)
            {
                listUser = listUser + item.User + "(" + ((CameraSourceType)obj.Item2[0].Source).ToDescription() + ")" + ",";
            }
            LstUser = listUser;
        }

        public ICommand PushToListCameraPageCommand
        {
            get
            {
                return new Command(() =>
                {
                    SafeExecute(async () =>
                    {
                        if (CheckPermision((int)PermissionKeyNames.AdminUtilityImageView))
                        {
                            await NavigationService.GoBackAsync();

                            var parameters = new NavigationParameters();
                            parameters.Add(ParameterKey.Vehicle, new Vehicle()
                            {
                                VehicleId = Vehicle.VehicleId,
                                VehiclePlate = Vehicle.VehiclePlate,
                                PrivateCode = Vehicle.PrivateCode
                            });
                            await NavigationService.NavigateAsync("NavigationPage/ListCameraVehicle", parameters, true, true);
                        }
                        else
                        {
                            DisplayMessage.ShowMessageInfo(MobileResource.Common_Message_NotPermission);
                        }
                    });
                });
            }
        }

        public ICommand StopPlaybackCommand
        {
            get
            {
                return new Command(() =>
                {
                    StopStreaming();
                });
            }
        }

        public ICommand ViewUserCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsShowUser = !IsShowUser;
                });
            }
        }

        private void StopStreaming()
        {
            SafeExecute(async () =>
            {
                var start = new CameraStopRequest()
                {
                    Channel = 15,
                    CustomerID = UserInfo.XNCode,
                    VehicleName = Vehicle.VehiclePlate,
                    Source = (int)CameraSourceType.App,
                    User = UserInfo.UserName,
                    SessionID = StaticSettings.SessionID
                };
                DependencyService.Get<IHUDProvider>().DisplayProgress("");
                await Task.Delay(TimeSpan.FromSeconds(5));
                var result = await _streamCameraService.DevicesStop(start);
                if (result)
                {
                    DependencyService.Get<IHUDProvider>().Dismiss();
                    if (StreamUserRequest != null && StreamUserRequest.Count > 0)
                    {
                        List<StreamUserRequest> lstUserSended = new List<StreamUserRequest>();
                        foreach (var item in StreamUserRequest)
                        {
                            if (item.User.ToUpper() != StaticSettings.User.UserName.ToUpper())
                            {
                                var isexist = lstUserSended.Exists(x => x.User == item.User);
                                if (!isexist)
                                {
                                    lstUserSended.Add(item);
                                    EventAggregator.GetEvent<UserMessageEvent>().Publish(new UserMessageEventModel()
                                    {
                                        UserName = item.User,
                                        Message = string.Format(MobileResource.Camera_Lable_CameraDisconnect,
                                        Vehicle.PrivateCode,
                                        UserInfo.UserName,
                                        ((CameraSourceType)item.Source).ToDescription())
                                    });
                                }
                            }
                        }
                    }
                    await NavigationService.GoBackAsync();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Camera_Message_StopStreamingOK, MobileResource.Common_Button_OK);
                    });
                }
                else
                {
                    DependencyService.Get<IHUDProvider>().Dismiss();
                }
            });
        }
    }
}