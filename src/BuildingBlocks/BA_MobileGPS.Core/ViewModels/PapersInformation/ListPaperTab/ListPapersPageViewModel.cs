using BA_MobileGPS.Service.IService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BA_MobileGPS.Core.ViewModels
{
    public class ListPapersPageViewModel : ViewModelBase
    {

        private readonly IPapersInforService paperinforService;
        public ListPapersPageViewModel(INavigationService navigationService, IPapersInforService paperinforService) : base(navigationService)
        {
            this.paperinforService = paperinforService;
        }
    }
}
