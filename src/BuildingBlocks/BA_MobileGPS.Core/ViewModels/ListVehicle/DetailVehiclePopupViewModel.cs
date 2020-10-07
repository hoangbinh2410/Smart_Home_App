using BA_MobileGPS.Core.Resources;
using BA_MobileGPS.Entities;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms.Extensions;

namespace BA_MobileGPS.Core.ViewModels
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
            InitMenuItems();
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

        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
            set
            {
                SetProperty(ref menuItems, value);
                RaisePropertyChanged();
            }
        }

        private void InitMenuItems()
        {
            var list = new List<MenuItem>();

            list.Add(new MenuItem
            {
                Title = MobileResource.Online_Label_TitlePage,
                Icon = "ic_mornitoring.png",
                IsEnable = true,
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.Route_Label_Title,
                Icon = "ic_route.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.ViewModuleRoute)
            });
            list.Add(new MenuItem
            {
                Title = MobileResource.DetailVehicle_Label_TilePage,
                Icon = "ic_guarantee.png",
                IsEnable = true,
            });
            list.Add(new MenuItem
            {
                Title = "Video",
                Icon = "ic_videolive.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingVideosView),
            });
            list.Add(new MenuItem
            {
                Title = "Hình Ảnh",
                Icon = "ic_cameraonline.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.TrackingOnlineByImagesView),
            });
            list.Add(new MenuItem
            {
                Title = "Nhiên liệu",
                Icon = "ic_fuel.png",
                IsEnable = CheckPermision((int)PermissionKeyNames.ReportFuelView),
            });
            MenuItems = list.Where(x => x.IsEnable == true).ToObservableCollection();
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