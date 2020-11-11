using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace BA_MobileGPS.Core.ViewModels
{
    public class CameraRestreamViewModel : ViewModelBase
    {
        public ICommand SelectDeviceTabCommand { get; }
        public ICommand SelectBACloudTabCommand { get; }
        public ICommand SelectMyVideoTabCommand { get; }
        public CameraRestreamViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectDeviceTabCommand = new DelegateCommand(SelectDeviceTab);
            SelectBACloudTabCommand = new DelegateCommand(SelectBACloudTab);
            SelectMyVideoTabCommand = new DelegateCommand(SelectMyVideoTab);
        }

        public override void OnPageAppearingFirstTime()
        {
            base.OnPageAppearingFirstTime();
            SelectDeviceTab();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        private void SelectMyVideoTab()
        {
            
        }

        private void SelectBACloudTab()
        {
            
        }

        private void SelectDeviceTab()
        {
            
        }

      
    }
}
