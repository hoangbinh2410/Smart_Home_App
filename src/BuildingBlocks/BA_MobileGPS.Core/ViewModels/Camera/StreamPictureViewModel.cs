using BA_MobileGPS.Entities;
using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms.Extensions;

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
            StreamSource = new ObservableCollection<StreamStart>();
            OKCommand = new DelegateCommand(OKClicked);
        }

        private VehicleOnline vehicleOnline { get; set; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<VehicleOnline>("Camera") is VehicleOnline cardetail)
            {
                vehicleOnline = cardetail;
                OKClicked();
            }           
        }

        private void ImageTap(object obj)
        {
            if (((ItemTappedEventArgs)obj).ItemData is StreamStart seletedChanel)
            {
                var parameters = new NavigationParameters
                {
                    { "Channel", seletedChanel },
                    { "Request", request }
                };

                NavigationService.NavigateAsync("DetailCamera", parameters, useModalNavigation: false);
            }
        }

        public ICommand ImageTapCommand { get; }
        private ObservableCollection<StreamStart> streamSource;

        public ObservableCollection<StreamStart> StreamSource
        {
            get { return streamSource; }
            set { SetProperty(ref streamSource, value); }
        }

        private string internalMessenger;
        public string InternalMessenger
        {
            get { return internalMessenger; }
            set
            {
                SetProperty(ref internalMessenger, value);
                RaisePropertyChanged();
            }
        }
        private string bKS;
        public string BKS
        {
            get { return bKS; }
            set { SetProperty(ref bKS, value); }
        }
        public ICommand OKCommand { get; }
        private StreamStartRequest request;
        private string vehiclePate = "CAMTEST2";
        private void GetCameraInfor(string bks)
        {
            using (new HUDService())
            {
                TryExecute(async () =>
                {
                    var statusResponse = await streamCameraService.GetDevicesStatus((int)ConditionType.BKS, bks);

                    if (statusResponse.Data != null && statusResponse.Data.Count > 0)
                    {
                        var temp = new List<StreamStart>();
                        foreach (var data in statusResponse.Data)
                        {
                            request = new StreamStartRequest()
                            {
                                //Channel = data.CameraChannel,
                                xnCode = data.XnCode,
                                VehiclePlate = data.VehiclePlate
                            };
                            var camResponse = await streamCameraService.StartStream(request);
                            temp.AddRange(camResponse.Data);
                        }
                        StreamSource = temp.ToObservableCollection();
                    }
                    else
                    {
                        InternalMessenger = statusResponse.UserMessage;
                    }
                });
            }
        }
        public override void OnResume()
        {
            base.OnResume();
            OKClicked();
        }

        private void OKClicked()
        {
            if (string.IsNullOrEmpty(BKS))
            {
                GetCameraInfor(vehiclePate);
            }
            else GetCameraInfor(BKS);
        }
    }




}