﻿using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public enum ConditionType
    {
        MXN = 1,
        BKS = 2,
        IMEI = 3
    }

    public class StreamPictureViewModel : ViewModelBase
    {
        private readonly IStreamCameraService streamCameraService;

        public StreamPictureViewModel(INavigationService navigationService, IStreamCameraService streamCameraService) : base(navigationService)
        {
            this.streamCameraService = streamCameraService;
            ImageTapCommand = new DelegateCommand<object>(ImageTap);
            StreamSource = new List<StreamStart>();
        }

        private VehicleOnline vehicleOnline { get; set; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<VehicleOnline>("Camera") is VehicleOnline cardetail)
            {
                vehicleOnline = cardetail;
            }
            GetCameraInfor();
        }

        private void ImageTap(object obj)
        {
            if (((ItemTappedEventArgs)obj).ItemData is StreamStart seletedChanel)
            {
                var parameters = new NavigationParameters
                {
                    { "Channel", seletedChanel.Link }
                };
                NavigationService.NavigateAsync("DetailCamera", parameters, useModalNavigation: false);
            }                   
        }

        public ICommand ImageTapCommand { get; }
        private List<StreamStart> streamSource;

        public List<StreamStart> StreamSource
        {
            get { return streamSource; }
            set { SetProperty(ref streamSource, value); }
        }

        private string internalMessenger;
        public string InternalMessenger
        {
            get { return internalMessenger; }
            set { SetProperty(ref internalMessenger, value); 
                RaisePropertyChanged(); }
        }


        private string custommerId = "1010";
        private void GetCameraInfor()
        {
            using (new HUDService())
            {
                TryExecute(async () =>
                {
                    var statusResponse = await streamCameraService.GetDevicesStatus((int)ConditionType.MXN, custommerId);

                    if (statusResponse.Data != null && statusResponse.Data.Count > 0)
                    {
                        var data = statusResponse.Data[0];
                        // var camActive = Convert.ToString(camChanel, 2);
                        var camResponse = await streamCameraService.StartStream(new StreamStartRequest()
                        {
                            Channel = data.CameraChannel,
                            //Channel = 15,
                            CustomerID = Convert.ToInt32(data.CustomerID),
                            VehicleName = data.VehicleName
                        });
                        StreamSource = camResponse.Data;
                    }
                    else
                    {
                        InternalMessenger = statusResponse.InternalMessage;
                    }
                });
            }
        }
    }




}