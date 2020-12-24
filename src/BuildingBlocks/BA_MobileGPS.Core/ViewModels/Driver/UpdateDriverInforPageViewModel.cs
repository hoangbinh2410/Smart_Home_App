using BA_MobileGPS.Core.Constant;
using BA_MobileGPS.Entities;
using BA_MobileGPS.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class UpdateDriverInforPageViewModel : ViewModelBase
    {
        private readonly IDriverInforService driverInforService;
        public UpdateDriverInforPageViewModel(INavigationService navigationService, IDriverInforService driverInforService) : base(navigationService)
        {
            this.driverInforService = driverInforService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(ParameterKey.DriverInformation, out DriverInfor driver))
            {
                
            }
        }
    }
}
