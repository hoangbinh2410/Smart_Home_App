using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms.Extensions;
using BA_MobileGPS.Core.ViewModels;

namespace MOTO_MobileGPS.ViewModels
{
    public class DetailVehiclePopupViewModel : ViewModelBase
    {
        public DetailVehiclePopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            CloseCommand = new DelegateCommand(Close);
            NavigativeCommand = new DelegateCommand<object>(Navigative);
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
                if (parameters.TryGetValue("vehicleItem", out string obj))
                {
                    VehicleName = obj;
                }
            }
        }

        private string vehicleName;

        public string VehicleName
        {
            get { return vehicleName; }
            set { SetProperty(ref vehicleName, value); }
        }

        public ICommand CloseCommand { get; }

        private void Close()
        {
            NavigationService.GoBackAsync();
        }

        public ICommand NavigativeCommand { get; }

        private void Navigative(object obj)
        {
            if (!(obj is MenuItem seletedMenu))
            {
                return;
            }
            SafeExecute(async () =>
            {

                var param = seletedMenu.Title.ToString();
                await NavigationService.GoBackAsync(useModalNavigation: true, parameters: new NavigationParameters
                        {
                            { "pagetoNavigation",  param}
                        });
            });

        }
    }
}