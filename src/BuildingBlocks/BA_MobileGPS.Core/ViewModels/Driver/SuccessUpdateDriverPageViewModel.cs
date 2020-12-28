using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class SuccessUpdateDriverPageViewModel : ViewModelBase
    {
        public SuccessUpdateDriverPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            ContinueInsertCommand = new DelegateCommand(ContinueInsert);
            CloseCommand = new DelegateCommand(Close);
        }

        private void Close()
        {
            NavigationService.GoBackToRootAsync();
        }

        private void ContinueInsert()
        {
            NavigationService.NavigateAsync("NavigationPage/UpdateDriverInforPage", null, true, true);
        }

        public ICommand ContinueInsertCommand { get; }
        public ICommand CloseCommand { get; }
    }
}
