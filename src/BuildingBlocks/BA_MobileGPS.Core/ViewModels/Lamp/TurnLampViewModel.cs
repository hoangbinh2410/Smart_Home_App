using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BA_MobileGPS.Core.ViewModels
{
   public class TurnLampViewModel : ViewModelBase
    {
        public TurnLampViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Hệ thống đèn";
        }
    }
}