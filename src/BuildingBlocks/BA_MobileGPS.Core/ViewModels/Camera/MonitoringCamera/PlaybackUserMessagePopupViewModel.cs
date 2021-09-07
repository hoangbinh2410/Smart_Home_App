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
    public class PlaybackUserMessagePopupViewModel : ViewModelBase
    {
        private readonly IStreamCameraService _streamCameraService;

        public PlaybackUserMessagePopupViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            Title = MobileResource.Common_Label_Notification;
            _streamCameraService = streamCameraService;
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
                if (parameters.TryGetValue("PlaybackUser", out Tuple<Vehicle, List<PlaybackUserRequest>> obj))
                {
                    Vehicle = obj.Item1;
                    PlaybackUserRequest = obj.Item2;
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

        private List<PlaybackUserRequest> playbackUserRequest = new List<PlaybackUserRequest>();
        public List<PlaybackUserRequest> PlaybackUserRequest { get => playbackUserRequest; set => SetProperty(ref playbackUserRequest, value); }

        private string _massage;

        public string Message
        {
            get { return _massage; }
            set { SetProperty(ref _massage, value); }
        }

        private void ShowMessage(Tuple<Vehicle, List<PlaybackUserRequest>> obj)
        {
            Message = string.Format(MobileResource.Camera_Message_DevicePlaybackErrorDetail,
                   obj.Item1.PrivateCode,
                   obj.Item2[0].User,
                   ((CameraSourceType)obj.Item2[0].Source).ToDescription(),
                    obj.Item2[0].User);
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
                    StopPlayback();
                });
            }
        }

        /// <summary>
        /// Gọi api stop streaming
        /// </summary>
        /// <param name="req"></param>
        private void StopPlayback()
        {
            SafeExecute(async () =>
            {
                var start = new PlaybackStopRequest()
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
                var result = await _streamCameraService.StopAllPlayback(start);
                if (result)
                {
                    DependencyService.Get<IHUDProvider>().Dismiss();
                    if (PlaybackUserRequest != null && PlaybackUserRequest.Count > 0)
                    {
                        List<PlaybackUserRequest> lstUserSended = new List<PlaybackUserRequest>();
                        foreach (var item in PlaybackUserRequest)
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
                                        Message = string.Format(MobileResource.Camera_Lable_PlaybackDisconnect, Vehicle.PrivateCode,
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
                        await PageDialog.DisplayAlertAsync(MobileResource.Common_Label_Notification, MobileResource.Camera_Message_StopPlaybackOK, MobileResource.Common_Button_OK);
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