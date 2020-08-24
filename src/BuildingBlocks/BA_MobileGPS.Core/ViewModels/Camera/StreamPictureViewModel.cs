using BA_MobileGPS.Entities;
using LibVLCSharp.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BA_MobileGPS.Core.ViewModels
{
    public class StreamPictureViewModel : ViewModelBase
    {
        public StreamPictureViewModel(INavigationService navigationService) : base(navigationService)
        {
            ImageTapCommand = new DelegateCommand<object>(ImageTap);
        }
        private VehicleOnline vehicleOnline { get; set; }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters?.GetValue<VehicleOnline>("Camera") is VehicleOnline cardetail)
            {
                vehicleOnline = cardetail;
            }          
        }

        private void ImageTap(object index)
        {
            NavigationService.NavigateAsync("DetailCamera", useModalNavigation:false);
        }

        public ICommand ImageTapCommand { get; }

     

       
    }
}
