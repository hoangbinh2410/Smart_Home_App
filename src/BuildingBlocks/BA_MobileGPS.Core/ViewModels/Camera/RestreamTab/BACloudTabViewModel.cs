﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class BACloudTabViewModel : ViewModelBase
    {
        public BACloudTabViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}